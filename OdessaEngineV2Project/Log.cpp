
//#ifndef TARGET_OS_MAC
#include "Stdafx.h"
//#endif

#include <stdarg.h>
#include <iostream>
#include <time.h>

#ifdef WIN32
#include <tchar.h>
#endif

Log::Log(const char* filename)
{
    isInitialized = false;
    
	if (filename != NULL)
	{
		m_stream.open(filename);
        isInitialized = true;
		cout << "OdessaEngineV2: Log file: " << filename << endl;
	}

	//int errno;
	//errno = fopen_s(&pLogFile, filename, "w");
	//if (pLogFile == NULL)
	//	cout << "Error opening logpath for writing: " << errno << endl;

}

#ifdef WIN32
void Log::Write(const wchar_t* logline, ...){

	const char* buffer = GetTime();
	
	
	va_list argList;
	wchar_t cbuffer[1024];
	va_start(argList, logline);
#ifdef WIN32
	vswprintf_s(cbuffer, 1024, logline, argList); // vsnprintf_s
#else
    vsnprintf(cbuffer, 1024, logline, argList);
#endif
	va_end(argList);
	
    if (isInitialized)
	{
		m_stream << buffer << " - " << cbuffer << endl;
        m_stream.flush();
	}

	wcout << buffer << " - " << cbuffer << endl;
	
}


void Log::Write(const LPWSTR logline){

	const char* buffer = GetTime();
	
	
    if (isInitialized)
	{
		m_stream << buffer << " - " << logline << endl;
        m_stream.flush();
	}

	wcout << buffer << " - " << logline << endl;
	
}
#endif



void Log::Write(const char* logline, ...){

	const char* buffer = GetTime();
	
	
	va_list argList;
	char cbuffer[1024];
	va_start(argList, logline);
#ifdef WIN32
	vsnprintf_s(cbuffer, 1024, 1024, logline, argList); // vsnprintf_s
#else
    vsnprintf(cbuffer, 1024, logline, argList);
#endif
	va_end(argList);
	
    if (isInitialized)
	{
		m_stream << buffer << " - " << cbuffer << endl;
        m_stream.flush();
	}

	cout << buffer << " - " << cbuffer << endl;
	
    
}

char* Log::GetTime()
{
	time_t rawtime;
	int errno;
    static char buffer[80];
	
	time ( &rawtime );

#ifdef WIN32
	struct tm timeinfo;
	errno = localtime_s(&timeinfo,  &rawtime );
    strftime (buffer,80,"%I:%M:%S%p",&timeinfo);
#else
    struct tm * timeinfo;
    timeinfo = localtime(&rawtime);
    strftime (buffer,80,"%I:%M:%S%p",timeinfo);
#endif

	return buffer;
}


Log::~Log()
{
	if (isInitialized)
		m_stream.close();
	
}