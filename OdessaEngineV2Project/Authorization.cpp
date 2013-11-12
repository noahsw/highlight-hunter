#include "Stdafx.h"
#include "Authorization.h"

#include <vector>
#include "Crc32Dynamic.h"
#include <string>


#ifdef WIN32
#include <Windows.h>
#include <tchar.h>
#include <wincrypt.h>
#include <wintrust.h>
#include <mscat.h>
#include <wchar.h>
#include <SoftPub.h>
#pragma comment(lib, "wintrust")
#pragma comment(lib, "crypt32.lib")


//#include <DbgHelp.h>
//#include <Psapi.h>
#endif

#ifdef TARGET_OS_MAC
#include <mach-o/dyld.h>
#include <sys/param.h>
#define NO_ERROR 0
#define LPCTSTR const char *
#endif



using namespace std;


Authorization::Authorization(Log* log)
{
	pLog = log;
}

Authorization::~Authorization()
{
	// let the Engine clean up the logger
}


bool Authorization::IsAuthorized()
{

	bool valid;

#ifdef WIN32
	TCHAR szThisProcessPath[MAX_PATH];
	GetModuleFileName(NULL, szThisProcessPath, sizeof(szThisProcessPath) / sizeof(TCHAR));

	valid = isValidSignature(szThisProcessPath);

	#ifndef RELEASE // so we include test
		pLog->Write("IsAuthorized(): module path: ");
		pLog->Write(szThisProcessPath);
		valid = true; // so we don't have to sign debug build
	#endif

#endif

	
#ifdef TARGET_OS_MAC
    valid = true; // this is always true on Mac because we don't use a separate dylib anymore. The security isn't necessary.
#endif

	if (valid)
		pLog->Write("IsAuthorized(): returning true");
	else
		pLog->Write("IsAuthorized(): returning false");
	
	return valid;

}


#ifdef WIN32

bool Authorization::isValidSignature(LPCTSTR szFileName)
{
	bool isValid = false;

    //WCHAR szFileName[MAX_PATH]; 
    HCERTSTORE hStore = NULL;
    HCRYPTMSG hMsg = NULL; 
    PCCERT_CONTEXT pCertContext = NULL;
    BOOL fResult;   
    DWORD dwEncoding, dwContentType, dwFormatType;
    PCMSG_SIGNER_INFO pSignerInfo = NULL;
    PCMSG_SIGNER_INFO pCounterSignerInfo = NULL;
    DWORD dwSignerInfo;
    CERT_INFO CertInfo;     
    SPROG_PUBLISHERINFO ProgPubInfo;

    ZeroMemory(&ProgPubInfo, sizeof(ProgPubInfo));
    __try
    {

		/*
		#ifdef UNICODE
        lstrcpynW(szFileName, szFileNameTCHAR, MAX_PATH);
#else
        if (mbstowcs(szFileName, szFileNameTCHAR, MAX_PATH) == -1)
        {
            printf("Unable to convert to unicode.\n");
            __leave;
        }
#endif
		*/

        // Get message handle and store handle from the signed file.
        fResult = CryptQueryObject(CERT_QUERY_OBJECT_FILE,
                                   szFileName,
                                   CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED_EMBED,
                                   CERT_QUERY_FORMAT_FLAG_BINARY,
                                   0,
                                   &dwEncoding,
                                   &dwContentType,
                                   &dwFormatType,
                                   &hStore,
                                   &hMsg,
                                   NULL);
        if (!fResult)
        {
			pLog->Write("CryptQueryObject failed with %x\n", GetLastError());
            __leave;
        }

        // Get signer information size.
        fResult = CryptMsgGetParam(hMsg, 
                                   CMSG_SIGNER_INFO_PARAM, 
                                   0, 
                                   NULL, 
                                   &dwSignerInfo);
        if (!fResult)
        {
            pLog->Write("CryptMsgGetParam failed with %x\n", GetLastError());
            __leave;
        }

        // Allocate memory for signer information.
        pSignerInfo = (PCMSG_SIGNER_INFO)LocalAlloc(LPTR, dwSignerInfo);
        if (!pSignerInfo)
        {
            pLog->Write("Unable to allocate memory for Signer Info.\n");
            __leave;
        }

        // Get Signer Information.
        fResult = CryptMsgGetParam(hMsg, 
                                   CMSG_SIGNER_INFO_PARAM, 
                                   0, 
                                   (PVOID)pSignerInfo, 
                                   &dwSignerInfo);
        if (!fResult)
        {
            pLog->Write("CryptMsgGetParam failed with %x\n", GetLastError());
            __leave;
        }
        
        // Get program name and publisher information from 
        // signer info structure.
		/*
        if (GetProgAndPublisherInfo(pSignerInfo, &ProgPubInfo))
        {
            if (ProgPubInfo.lpszProgramName != NULL)
            {
                wprintf(L"Program Name : %s\n",
                    ProgPubInfo.lpszProgramName);
            }

            if (ProgPubInfo.lpszPublisherLink != NULL)
            {
                wprintf(L"Publisher Link : %s\n",
                    ProgPubInfo.lpszPublisherLink);
            }

            if (ProgPubInfo.lpszMoreInfoLink != NULL)
            {
                wprintf(L"MoreInfo Link : %s\n",
                    ProgPubInfo.lpszMoreInfoLink);
            }
        }
		*/

        pLog->Write("\n");

        // Search for the signer certificate in the temporary 
        // certificate store.
        CertInfo.Issuer = pSignerInfo->Issuer;
        CertInfo.SerialNumber = pSignerInfo->SerialNumber;

        pCertContext = CertFindCertificateInStore(hStore,
                                                  ENCODING,
                                                  0,
                                                  CERT_FIND_SUBJECT_CERT,
                                                  (PVOID)&CertInfo,
                                                  NULL);
        if (!pCertContext)
        {
            pLog->Write("CertFindCertificateInStore failed with %x\n",
                GetLastError());
            __leave;
        }

        // Print Signer certificate information.
#ifdef DEBUG
        pLog->Write("Signer Certificate:");
#endif
        isValid = isValidCertificateInfo(pCertContext);
      
		/* We don't use the timestamp certificate
        // Get the timestamp certificate signerinfo structure.
	    SYSTEMTIME st;
        if (GetTimeStampSignerInfo(pSignerInfo, &pCounterSignerInfo))
        {
            // Search for Timestamp certificate in the temporary
            // certificate store.
            CertInfo.Issuer = pCounterSignerInfo->Issuer;
            CertInfo.SerialNumber = pCounterSignerInfo->SerialNumber;

            pCertContext = CertFindCertificateInStore(hStore,
                                                ENCODING,
                                                0,
                                                CERT_FIND_SUBJECT_CERT,
                                                (PVOID)&CertInfo,
                                                NULL);
            if (!pCertContext)
            {
                pLog->Write("CertFindCertificateInStore failed with %x\n",
                    GetLastError());
                __leave;
            }

            // Print timestamp certificate information.
            pLog->Write("TimeStamp Certificate:");
            PrintCertificateInfo(pCertContext);
            pLog->Write("\n");

            // Find Date of timestamp.
            if (GetDateOfTimeStamp(pCounterSignerInfo, &st))
            {
                pLog->Write("Date of TimeStamp : %02d/%02d/%04d %02d:%02d\n",
                                            st.wMonth,
                                            st.wDay,
                                            st.wYear,
                                            st.wHour,
                                            st.wMinute);
            }
            pLog->Write("\n");
        }
		*/
    }
    __finally
    {               
        // Clean up.
        if (ProgPubInfo.lpszProgramName != NULL)
            LocalFree(ProgPubInfo.lpszProgramName);
        if (ProgPubInfo.lpszPublisherLink != NULL)
            LocalFree(ProgPubInfo.lpszPublisherLink);
        if (ProgPubInfo.lpszMoreInfoLink != NULL)
            LocalFree(ProgPubInfo.lpszMoreInfoLink);

        if (pSignerInfo != NULL) LocalFree(pSignerInfo);
        if (pCounterSignerInfo != NULL) LocalFree(pCounterSignerInfo);
        if (pCertContext != NULL) CertFreeCertificateContext(pCertContext);
        if (hStore != NULL) CertCloseStore(hStore, 0);
        if (hMsg != NULL) CryptMsgClose(hMsg);
    }

    return isValid;
}



bool Authorization::isValidCertificateInfo(PCCERT_CONTEXT pCertContext)
{
    bool fReturn = false;
    LPWSTR szName = NULL;
    DWORD dwData;

	bool isValid = false;

    __try
    {
        // Print Serial Number.
		/*
        pLog->Write("Serial Number: ");
        dwData = pCertContext->pCertInfo->SerialNumber.cbData;
        for (DWORD n = 0; n < dwData; n++)
        {
            pLog->Write("%02x",
              pCertContext->pCertInfo->SerialNumber.pbData[dwData - (n + 1)]);
        }
        pLog->Write("\n");
		*/

        // Get Issuer name size.
        if (!(dwData = CertGetNameString(pCertContext, 
                                         CERT_NAME_SIMPLE_DISPLAY_TYPE,
                                         CERT_NAME_ISSUER_FLAG,
                                         NULL,
                                         NULL,
                                         0)))
        {
            pLog->Write("CertGetNameString failed.\n");
            __leave;
        }

        // Allocate memory for Issuer name.
        szName = (LPTSTR)LocalAlloc(LPTR, dwData * sizeof(TCHAR));
        if (!szName)
        {
            pLog->Write("Unable to allocate memory for issuer name.\n");
            __leave;
        }

        // Get Issuer name.
        if (!(CertGetNameString(pCertContext, 
                                CERT_NAME_SIMPLE_DISPLAY_TYPE,
                                CERT_NAME_ISSUER_FLAG,
                                NULL,
                                szName,
                                dwData)))
        {
            pLog->Write("CertGetNameString failed.\n");
            __leave;
        }

        // print Issuer name.
#ifndef RELEASE
        pLog->Write("Issuer Name:");
		pLog->Write(szName);
#endif

		wchar_t validIssuerName[] = L"COMODO Code Signing CA 2";
		int cmp = wcscmp(validIssuerName, szName);
		if (cmp == 0)
			isValid = true;
		
        LocalFree(szName);
        szName = NULL;

        // Get Subject name size.
        if (!(dwData = CertGetNameString(pCertContext, 
                                         CERT_NAME_SIMPLE_DISPLAY_TYPE,
                                         0,
                                         NULL,
                                         NULL,
                                         0)))
        {
            pLog->Write("CertGetNameString failed.\n");
            __leave;
        }

        // Allocate memory for subject name.
        szName = (LPWSTR)LocalAlloc(LPTR, dwData * sizeof(TCHAR));
        if (!szName)
        {
            pLog->Write("Unable to allocate memory for subject name.\n");
            __leave;
        }

        // Get subject name.
        if (!(CertGetNameString(pCertContext, 
                                CERT_NAME_SIMPLE_DISPLAY_TYPE,
                                0,
                                NULL,
                                szName,
                                dwData)))
        {
            pLog->Write("CertGetNameString failed.\n");
            __leave;
        }

        // Print Subject Name.
#ifndef RELEASE
        pLog->Write("Subject Name:");
		pLog->Write(szName);
#endif

		wchar_t validSubjectName[] = L"Authentically Digital";
		cmp = wcscmp(validSubjectName, szName);
		if (cmp == 0 && isValid == true)
			isValid = true;

    }
    __finally
    {
        if (szName != NULL) LocalFree(szName);
    }

    return isValid;
}

LPWSTR AllocateAndCopyWideString(LPCWSTR inputString)
{
    LPWSTR outputString = NULL;

    outputString = (LPWSTR)LocalAlloc(LPTR,
        (wcslen(inputString) + 1) * sizeof(WCHAR));
    if (outputString != NULL)
    {
        lstrcpyW(outputString, inputString);
    }
    return outputString;
}

BOOL Authorization::GetProgAndPublisherInfo(PCMSG_SIGNER_INFO pSignerInfo, PSPROG_PUBLISHERINFO Info)
{
    BOOL fReturn = FALSE;
    PSPC_SP_OPUS_INFO OpusInfo = NULL;  
    DWORD dwData;
    BOOL fResult;
    
    __try
    {
        // Loop through authenticated attributes and find
        // SPC_SP_OPUS_INFO_OBJID OID.
        for (DWORD n = 0; n < pSignerInfo->AuthAttrs.cAttr; n++)
        {           
            if (lstrcmpA(SPC_SP_OPUS_INFO_OBJID, 
                        pSignerInfo->AuthAttrs.rgAttr[n].pszObjId) == 0)
            {
                // Get Size of SPC_SP_OPUS_INFO structure.
                fResult = CryptDecodeObject(ENCODING,
                            SPC_SP_OPUS_INFO_OBJID,
                            pSignerInfo->AuthAttrs.rgAttr[n].rgValue[0].pbData,
                            pSignerInfo->AuthAttrs.rgAttr[n].rgValue[0].cbData,
                            0,
                            NULL,
                            &dwData);
                if (!fResult)
                {
                    pLog->Write("CryptDecodeObject failed with %x\n",
                        GetLastError());
                    __leave;
                }

                // Allocate memory for SPC_SP_OPUS_INFO structure.
                OpusInfo = (PSPC_SP_OPUS_INFO)LocalAlloc(LPTR, dwData);
                if (!OpusInfo)
                {
                    pLog->Write("Unable to allocate memory for Publisher Info.\n");
                    __leave;
                }

                // Decode and get SPC_SP_OPUS_INFO structure.
                fResult = CryptDecodeObject(ENCODING,
                            SPC_SP_OPUS_INFO_OBJID,
                            pSignerInfo->AuthAttrs.rgAttr[n].rgValue[0].pbData,
                            pSignerInfo->AuthAttrs.rgAttr[n].rgValue[0].cbData,
                            0,
                            OpusInfo,
                            &dwData);
                if (!fResult)
                {
                    pLog->Write("CryptDecodeObject failed with %x\n",
                        GetLastError());
                    __leave;
                }

                // Fill in Program Name if present.
                if (OpusInfo->pwszProgramName)
                {
                    Info->lpszProgramName =
                        AllocateAndCopyWideString(OpusInfo->pwszProgramName);
                }
                else
                    Info->lpszProgramName = NULL;

                // Fill in Publisher Information if present.
                if (OpusInfo->pPublisherInfo)
                {

                    switch (OpusInfo->pPublisherInfo->dwLinkChoice)
                    {
                        case SPC_URL_LINK_CHOICE:
                            Info->lpszPublisherLink =
                                AllocateAndCopyWideString(OpusInfo->pPublisherInfo->pwszUrl);
                            break;

                        case SPC_FILE_LINK_CHOICE:
                            Info->lpszPublisherLink =
                                AllocateAndCopyWideString(OpusInfo->pPublisherInfo->pwszFile);
                            break;

                        default:
                            Info->lpszPublisherLink = NULL;
                            break;
                    }
                }
                else
                {
                    Info->lpszPublisherLink = NULL;
                }

                // Fill in More Info if present.
                if (OpusInfo->pMoreInfo)
                {
                    switch (OpusInfo->pMoreInfo->dwLinkChoice)
                    {
                        case SPC_URL_LINK_CHOICE:
                            Info->lpszMoreInfoLink =
                                AllocateAndCopyWideString(OpusInfo->pMoreInfo->pwszUrl);
                            break;

                        case SPC_FILE_LINK_CHOICE:
                            Info->lpszMoreInfoLink =
                                AllocateAndCopyWideString(OpusInfo->pMoreInfo->pwszFile);
                            break;

                        default:
                            Info->lpszMoreInfoLink = NULL;
                            break;
                    }
                }               
                else
                {
                    Info->lpszMoreInfoLink = NULL;
                }

                fReturn = TRUE;

                break; // Break from for loop.
            } // lstrcmp SPC_SP_OPUS_INFO_OBJID                 
        } // for 
    }
    __finally
    {
        if (OpusInfo != NULL) LocalFree(OpusInfo);      
    }

    return fReturn;
}

BOOL Authorization::GetDateOfTimeStamp(PCMSG_SIGNER_INFO pSignerInfo, SYSTEMTIME *st)
{   
    BOOL fResult;
    FILETIME lft, ft;   
    DWORD dwData;
    BOOL fReturn = FALSE;
    
    // Loop through authenticated attributes and find
    // szOID_RSA_signingTime OID.
    for (DWORD n = 0; n < pSignerInfo->AuthAttrs.cAttr; n++)
    {           
        if (lstrcmpA(szOID_RSA_signingTime, 
                    pSignerInfo->AuthAttrs.rgAttr[n].pszObjId) == 0)
        {               
            // Decode and get FILETIME structure.
            dwData = sizeof(ft);
            fResult = CryptDecodeObject(ENCODING,
                        szOID_RSA_signingTime,
                        pSignerInfo->AuthAttrs.rgAttr[n].rgValue[0].pbData,
                        pSignerInfo->AuthAttrs.rgAttr[n].rgValue[0].cbData,
                        0,
                        (PVOID)&ft,
                        &dwData);
            if (!fResult)
            {
                pLog->Write("CryptDecodeObject failed with %x\n",
                    GetLastError());
                break;
            }

            // Convert to local time.
            FileTimeToLocalFileTime(&ft, &lft);
            FileTimeToSystemTime(&lft, st);

            fReturn = TRUE;

            break; // Break from for loop.
                        
        } //lstrcmp szOID_RSA_signingTime
    } // for 

    return fReturn;
}

BOOL Authorization::GetTimeStampSignerInfo(PCMSG_SIGNER_INFO pSignerInfo, PCMSG_SIGNER_INFO *pCounterSignerInfo)
{   
    PCCERT_CONTEXT pCertContext = NULL;
    BOOL fReturn = FALSE;
    BOOL fResult;       
    DWORD dwSize;   

    __try
    {
        *pCounterSignerInfo = NULL;

        // Loop through unathenticated attributes for
        // szOID_RSA_counterSign OID.
        for (DWORD n = 0; n < pSignerInfo->UnauthAttrs.cAttr; n++)
        {
            if (lstrcmpA(pSignerInfo->UnauthAttrs.rgAttr[n].pszObjId, 
                         szOID_RSA_counterSign) == 0)
            {
                // Get size of CMSG_SIGNER_INFO structure.
                fResult = CryptDecodeObject(ENCODING,
                           PKCS7_SIGNER_INFO,
                           pSignerInfo->UnauthAttrs.rgAttr[n].rgValue[0].pbData,
                           pSignerInfo->UnauthAttrs.rgAttr[n].rgValue[0].cbData,
                           0,
                           NULL,
                           &dwSize);
                if (!fResult)
                {
                    pLog->Write("CryptDecodeObject failed with %x\n",
                        GetLastError());
                    __leave;
                }

                // Allocate memory for CMSG_SIGNER_INFO.
                *pCounterSignerInfo = (PCMSG_SIGNER_INFO)LocalAlloc(LPTR, dwSize);
                if (!*pCounterSignerInfo)
                {
                    pLog->Write("Unable to allocate memory for timestamp info.\n");
                    __leave;
                }

                // Decode and get CMSG_SIGNER_INFO structure
                // for timestamp certificate.
                fResult = CryptDecodeObject(ENCODING,
                           PKCS7_SIGNER_INFO,
                           pSignerInfo->UnauthAttrs.rgAttr[n].rgValue[0].pbData,
                           pSignerInfo->UnauthAttrs.rgAttr[n].rgValue[0].cbData,
                           0,
                           (PVOID)*pCounterSignerInfo,
                           &dwSize);
                if (!fResult)
                {
                    pLog->Write("CryptDecodeObject failed with %x\n",
                        GetLastError());
                    __leave;
                }

                fReturn = TRUE;
                
                break; // Break from for loop.
            }           
        }
    }
    __finally
    {
        // Clean up.
        if (pCertContext != NULL) CertFreeCertificateContext(pCertContext);
    }

    return fReturn;
}
	



bool Authorization::isValidCRC(LPCTSTR filepath)
{

#ifdef DEBUG
	//return true;
#endif

	// verify checksum on file
	DWORD dwCrc32;
	DWORD dwErrorCode = NO_ERROR;
	
	CCrc32Dynamic *pobCrc32Dynamic = new CCrc32Dynamic;
	pobCrc32Dynamic->Init();
	
#ifdef WIN32
	dwErrorCode = pobCrc32Dynamic->FileCrc32Assembly(filepath, dwCrc32);
#endif
	
#ifdef TARGET_OS_MAC
	dwErrorCode = pobCrc32Dynamic->FileCrc32Streams(filepath, dwCrc32);
#endif
	
	
	pobCrc32Dynamic->Free();
	delete pobCrc32Dynamic;
	
	if(dwErrorCode == NO_ERROR)
	{

		// windows
		//WCHAR thisCRC[32];
		wchar_t thisCRC[32];

		
#ifdef WIN32

		wchar_t validCRC[] = L"0x6ef8366f"; // PC (TRIM LEADING 0s AFTER THE 0x OTHERWISE COMPARISON WILL FAIL)
		swprintf_s(thisCRC, 32, L"0x%x", dwCrc32);
		
#endif
#ifdef TARGET_OS_MAC
		
		wchar_t validCRC[] = L"0xc18be683"; // MAC
		swprintf(thisCRC, 32, L"0x%x", dwCrc32);

#endif

		

		// compare against whitelist
		//WCHAR valid[32] = _TEXT("738919885");

		/*
		bool equal = true;
		wcout << wcslen(validCRC);
		wcout << " = ";
		wcout << wcslen(thisCRC);
		wcout << endl;
		for (int i = 0; i < wcslen(validCRC); i++)
		{
			wcout << i;
			wcout << ": ";
			wcout << validCRC[i];
			wcout << " = ";
			wcout << thisCRC[i];
			wcout << endl;
			//if (validCRC[i] == '\0' && thisCRC[i] == '\0')
			//	break;
			
			if (validCRC[i] != thisCRC[i])
			{
				equal = false;
				break;
			}

		}
		*/

		
		int cmp = 0;
		cmp = wcscmp(validCRC, thisCRC);
#ifndef RELEASE
		pLog->Write("value of validCRC: %lc", validCRC);
		pLog->Write("len of validCRC: %d", wcslen(validCRC));
#endif
		pLog->Write("value of thisCRC:");
#ifdef WIN32
		pLog->Write(thisCRC);
#else
		pLog->Write("%ls", thisCRC);
#endif
		//pLog->Write("len of thisCRC: %d", wcslen(thisCRC));
		//pLog->Write("value of cmp: %d", cmp);
		
		if (cmp == 0)
		{
			pLog->Write("returning true");
			return true;
		}
		else
		{
			pLog->Write("returning false");
			return false;
		}

	}
	else
	{
		return false;
	}

}


#endif // #ifdef WIN32