#include "Stdafx.h"

#ifdef WIN32
#include <Windows.h>
#include <vector>
#include <tchar.h>


#define ENCODING (X509_ASN_ENCODING | PKCS_7_ASN_ENCODING)

typedef struct {
    LPWSTR lpszProgramName;
    LPWSTR lpszPublisherLink;
    LPWSTR lpszMoreInfoLink;
} SPROG_PUBLISHERINFO, *PSPROG_PUBLISHERINFO;

#endif

#ifdef TARGET_OS_MAC
#define LPCTSTR const char *
#endif

#include <string>


using namespace std;

class Authorization {
    public:
		Authorization(Log* log);
		~Authorization();
		bool IsAuthorized();

	private:
		Log* pLog;
		bool isValidCRC(LPCTSTR filepath);
		bool isValidSignature(LPCTSTR szFileNameTCHAR);

#ifdef WIN32
		bool isValidCertificateInfo(PCCERT_CONTEXT pCertContext);
		BOOL GetProgAndPublisherInfo(PCMSG_SIGNER_INFO pSignerInfo, PSPROG_PUBLISHERINFO Info);
		BOOL GetDateOfTimeStamp(PCMSG_SIGNER_INFO pSignerInfo, SYSTEMTIME *st);
		BOOL GetTimeStampSignerInfo(PCMSG_SIGNER_INFO pSignerInfo, PCMSG_SIGNER_INFO *pCounterSignerInfo);
#endif

};