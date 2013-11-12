#!/bin/sh

# WARNING: You may have to run Clean in Xcode after changing CODE_SIGN_IDENTITY!

# Verify that $CODE_SIGN_IDENTITY is set
if [ -z "$CODE_SIGN_IDENTITY" ]; then
echo "CODE_SIGN_IDENTITY needs to be non-empty for codesigning frameworks!"

if [ "$CONFIGURATION" = "Release" ]; then
exit 1
else
# Codesigning is optional for non-release builds.
exit 0
fi
fi

echo "note: Running script"

# FFMPEG
#echo "note: codesign -v --entitlements '${SRCROOT}/ffmpeg.entitlements' '${BUILT_PRODUCTS_DIR}/${PRODUCT_NAME}.app/Contents/Resources/ffmpeg'" # -f -s '${CODE_SIGN_IDENTITY}'
codesign -s "${CODE_SIGN_IDENTITY}" --entitlements "${SRCROOT}/ffmpeg.entitlements" "${BUILT_PRODUCTS_DIR}/${PRODUCT_NAME}.app/Contents/Resources/ffmpeg"
codesign --display --entitlements - "${BUILT_PRODUCTS_DIR}/${PRODUCT_NAME}.app/Contents/Resources/ffmpeg"

# MEDIAINFO
#echo "note: codesign -v --entitlements '${SRCROOT}/mediainfo.entitlements' '${BUILT_PRODUCTS_DIR}/${PRODUCT_NAME}.app/Contents/Resources/mediainfo'" # -f -s '${CODE_SIGN_IDENTITY}'
codesign -s "${CODE_SIGN_IDENTITY}" --entitlements "${SRCROOT}/mediainfo.entitlements" "${BUILT_PRODUCTS_DIR}/${PRODUCT_NAME}.app/Contents/Resources/mediainfo"
codesign --display --entitlements - "${BUILT_PRODUCTS_DIR}/${PRODUCT_NAME}.app/Contents/Resources/mediainfo"




# Can't get frameworks to sign. Let's leave this alone until Apple complains about it.

#FRAMEWORK_DIR="${TARGET_BUILD_DIR}/${FRAMEWORKS_FOLDER_PATH}/"

#echo "note: Framework_DIR = ${FRAMEWORK_DIR}"
# see http://developer.apple.com/library/mac/#technotes/tn2206/_index.html
#codesign -f -v -s "${CODE_SIGN_IDENTITY}" "${FRAMEWORK_DIR}ASIHTTPRequest.framework/Versions/A"
#codesign -f -v -s "${CODE_SIGN_IDENTITY}" "${FRAMEWORK_DIR}FeedbackReporter.framework/Versions/A"
#codesign -f -v -s "${CODE_SIGN_IDENTITY}" "${FRAMEWORK_DIR}PhFacebook.framework/Versions/A"
#codesign -f -v -s "${CODE_SIGN_IDENTITY}" "${FRAMEWORK_DIR}SBJson.framework/Versions/A"



# This doesn't work because FRAMEWORK_DIR has a space in it
# Loop through all frameworks
#for FRAMEWORK in $(find "${FRAMEWORK_DIR}" -maxdepth 1 -type d -name "*.framework" );
#do
#echo "note: ${FRAMEWORK}"
#echo "note: codesign -f -v -s '${CODE_SIGN_IDENTITY}' '${FRAMEWORK}/Versions/A'"
#codesign -f -v -s "${CODE_SIGN_IDENTITY}" "${FRAMEWORK}/Versions/A"
#done