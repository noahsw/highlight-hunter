// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

//#include <stdio.h>
#include <iostream> // for writing to logger
//#include <sstream>
#include <time.h> // for calculating time in logger
#include <vector> // for returning list of frames
//#include <string>
//#include <cstdlib>

#ifndef Log_h
#define Log_h
#include "Log.h"
#endif

extern "C" {

	#include <avformat.h>
	#include <avutil.h>
	#include <avcodec.h>

	//#include <avdevice.h>
	//#include <avfilter.h>
	//#include <libswscale\swscale.h>

}