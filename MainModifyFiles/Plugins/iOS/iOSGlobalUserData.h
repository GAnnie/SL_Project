//
//  iOSGlobalUserData.h
//  BaoyugameSdk
//
//  Created by Mac-baoyu on 13-10-26.
//  Copyright (c) 2013年 Mac-baoyu. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface iOSGlobalUserData : NSObject

+ (iOSGlobalUserData *) sharedInstance;

+ (BOOL) addSkipBackupAttributeToFileAtPath:(NSString *)aFilePath;

+ (double) batteryLevel;

+ (BOOL) isBatteryCharging;

@end
