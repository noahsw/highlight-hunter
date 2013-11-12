# this ffmpeg-README file should be copied into the framework bundle's
# Resources directory along with the executable


# OLD WHEN WE WERE TRYING TO GET THIS WORKING ON 10.5
# ./configure --enable-static --disable-shared --disable-outdev=sdl --enable-runtime-cpudetect --disable-bzlib --disable-libfreetype --disable-libopenjpeg --enable-zlib --arch=x86_64 --sysroot=/Developer/SDKs/MacOSX10.7.sdk --extra-cflags="-isysroot /Developer/SDKs/MacOSX10.7.sdk -DMACOSX_DEPLOYMENT_TARGET=10.5 -mmacosx-version-min=10.5"

# CURRENT
# ./configure --enable-static --disable-shared --enable-gpl --enable-nonfree --enable-libx264 --disable-outdev=sdl --enable-runtime-cpudetect --disable-bzlib --disable-libfreetype --disable-libopenjpeg --enable-zlib --arch=x86_64 --sysroot=/Developer/SDKs/MacOSX10.7.sdk --extra-cflags="-isysroot /Developer/SDKs/MacOSX10.7.sdk"
# make


ffmpeg version 0.8.5, Copyright (c) 2000-2011 the FFmpeg developers
built on Nov  3 2011 10:58:55 with gcc 4.2.1 (Apple Inc. build 5666) (dot 3)
configuration: --enable-static --disable-shared --disable-outdev=sdl --enable-runtime-cpudetect --disable-bzlib --disable-libfreetype --disable-libopenjpeg --enable-zlib --arch=x86_64 --sysroot=/Developer/SDKs/MacOSX10.6.sdk --extra-cflags='-isysroot /Developer/SDKs/MacOSX10.6.sdk -DMACOSX_DEPLOYMENT_TARGET=10.5 -mmacosx-version-min=10.5'
libavutil    51.  9. 1 / 51.  9. 1
libavcodec   53.  7. 0 / 53.  7. 0
libavformat  53.  4. 0 / 53.  4. 0
libavdevice  53.  1. 1 / 53.  1. 1
libavfilter   2. 23. 0 /  2. 23. 0
libswscale    2.  0. 0 /  2.  0. 0



# CONFIGURE X264 (see http://hunterford.me/compiling-ffmpeg-on-mac-os-x/)
# CFLAGS="-I. -fno-common -read_only_relocs suppress" ./configure --enable-pic --enable-shared && make -j 4 && sudo make install;