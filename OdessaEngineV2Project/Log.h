#include <fstream>

#ifdef WIN32
#include <Windows.h>
#endif
 
using namespace std;
 
class Log {
    public:
        Log(const char* filename);
        ~Log();
        //void Write(char* logline);
        void Write(const wchar_t* logline, ...);
		void Write(const char* logline, ...);

#ifdef WIN32
		void Write(const LPWSTR logline);
#endif


    private:
        wofstream m_stream;
        bool isInitialized;
		char* GetTime();
 };