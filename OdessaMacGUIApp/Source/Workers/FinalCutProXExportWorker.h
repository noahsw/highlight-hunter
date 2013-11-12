//
//  FinalCutProExportWorker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 10/1/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "ExportWorker.h"
#import "DDLog.h"
#import "JFUrlUtil.h"

@interface FinalCutProXExportWorker : ExportWorker

+ (FinalCutProXExportWorker*)workerWithHighlights:(NSArray*)highlightObjects;


typedef struct {
    long nominator;
    long denominator;
    double error;
} Fraction;

Fraction fractionFromReal(double realNumber, long maxDenominator);

@end
