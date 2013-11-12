# this README file should be copied into the framework bundle's
# Libraries directory along with the dynamic libraries

# these ffmpeg libraries were configured like this:
# ./configure --disable-static --enable-shared --disable-outdev=sdl --enable-runtime-cpudetect --disable-bzlib --disable-libfreetype --disable-libopenjpeg --enable-zlib --cc="clang -m32"

# old:
#  ./configure --disable-static --enable-shared --disable-outdev=sdl --enable-runtime-cpudetect --disable-bzlib --disable-libfreetype --disable-libopenjpeg --enable-zlib --cc="clang -m32_64 "--sysroot=/Developer/SDKs/MacOSX10.6.sdk --extra-cflags="-isysroot /Developer/SDKs/MacOSX10.7.sdk -DMACOSX_DEPLOYMENT_TARGET=10.5 -mmacosx-version-min=10.5"

# old for 64-bit:
# ./configure --disable-static --enable-shared --disable-outdev=sdl --enable-runtime-cpudetect --disable-bzlib --disable-libfreetype --disable-libopenjpeg --enable-zlib --sysroot=/Developer/SDKs/MacOSX10.6.sdk --extra-cflags="-isysroot /Developer/SDKs/MacOSX10.7.sdk -arch x86_64"


# 2. Run 'make' (not make install)


# 3. copy dylibs and include headers



# if you replace these dynamic libraries with ones you build yourself
# you'll need to perform these commands to give them relative install paths

install_name_tool -id '@rpath/libavcodec.dylib' -change '/usr/local/lib/libavutil.dylib' '@rpath/libavutil.dylib' libavcodec.dylib

#install_name_tool -id '@rpath/libavdevice.dylib' -change '/usr/local/lib/libavformat.dylib' '@rpath/libavformat.dylib' -change '/usr/local/lib/libavcodec.dylib' '@rpath/libavcodec.dylib' -change '/usr/local/lib/libavutil.dylib' '@rpath/libavutil.dylib' libavdevice.dylib

#install_name_tool -id '@rpath/libavfilter.dylib' -change '/usr/local/lib/libavformat.dylib' '@rpath/libavformat.dylib' -change '/usr/local/lib/libavcodec.dylib' '@rpath/libavcodec.dylib' -change '/usr/local/lib/libswscale.dylib' '@rpath/libswscale.dylib' -change '/usr/local/lib/libavutil.dylib' '@rpath/libavutil.dylib' libavfilter.dylib

install_name_tool -id '@rpath/libavformat.dylib' -change '/usr/local/lib/libavcodec.dylib' '@rpath/libavcodec.dylib' -change '/usr/local/lib/libavutil.dylib' '@rpath/libavutil.dylib' libavformat.dylib

install_name_tool -id '@rpath/libavutil.dylib' -change '/usr/lib/libz.1.dylib' '@rpath/libz.1.dylib' libavutil.dylib

#install_name_tool -id '@rpath/libswscale.dylib' -change '/usr/local/lib/libavutil.dylib' '@rpath/libavutil.dylib' libswscale.dylib

# not sure we need libz.1.dylib if we already have libz.dylib.
#install_name_tool -id '@rpath/libz.1.dylib' -change '/usr/lib/libz.1.dylib' '@rpath/libz.1.dylib' libz.1.dylib

#install_name_tool -id '@rpath/libz.dylib' -change '/usr/lib/libz.dylib' '@rpath/libz.dylib' libz.dylib



# 4. update build settings
# 4.1 add dylibs (only aliases) and header files to project
# 4.2 add dylibs (only aliases) to Copy Files of framework (Executables \ Libraries)
# 4.3 add dylibs (only aliases) to Link Libraries
# 4.4 remove ffmpeg headers from include headers
# 4.5 remove README from being copied to build