#ifndef _CRC32DYNAMIC_H_
#define _CRC32DYNAMIC_H_

#include "Common.h"

#ifdef WIN32
    #include "Windows.h"
#endif

#ifdef TARGET_OS_MAC
    #include <string.h>
    typedef unsigned long DWORD;
    typedef unsigned char BYTE;
    typedef void* HANDLE;
    #define LPCTSTR const char *
#endif

class CCrc32Dynamic
{
	public:
		CCrc32Dynamic();
		virtual ~CCrc32Dynamic();

		void Init(void); // old: (void)
		void Free(void); // old: (void)

		DWORD FileCrc32Streams(LPCTSTR szFilename, DWORD &dwCrc32) const;
    
#ifdef WIN32
        DWORD StringCrc32(LPCTSTR szString, DWORD &dwCrc32) const;
        DWORD FileCrc32Win32(LPCTSTR szFilename, DWORD &dwCrc32) const;
		DWORD FileCrc32Filemap(LPCTSTR szFilename, DWORD &dwCrc32) const;
		DWORD FileCrc32Assembly(LPCTSTR szFilename, DWORD &dwCrc32) const;
#endif
    
	protected:
		static bool GetFileSizeQW(const HANDLE hFile, QWORD &qwSize);
		inline void CalcCrc32(const BYTE byte, DWORD &dwCrc32) const;

		DWORD *m_pdwCrc32Table;
};

#endif
