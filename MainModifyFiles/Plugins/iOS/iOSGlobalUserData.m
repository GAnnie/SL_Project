//
//  iOSGlobalUserData.m
//  BaoyugameSdk
//
//  Created by Mac-baoyu on 13-10-26.
//  Copyright (c) 2013年 Mac-baoyu. All rights reserved.
//

#import "iOSGlobalUserData.h"
#import <sys/xattr.h>
#import "IOPSKeys.h"
#import "IOPowerSources.h"

@implementation iOSGlobalUserData


+ (iOSGlobalUserData *) sharedInstance
{
    static iOSGlobalUserData *instance = nil;
    if ( instance == nil )
    {
        instance = [[iOSGlobalUserData alloc] init];
    }
    return instance;
}

+(double) batteryLevel
{
    
    //Returns a blob of Power Source information in an opaque CFTypeRef.
    CFTypeRef blob = IOPSCopyPowerSourcesInfo();
    
    //Returns a CFArray of Power Source handles, each of type CFTypeRef.
    CFArrayRef sources = IOPSCopyPowerSourcesList(blob);
    
    CFDictionaryRef pSource = NULL;
    const void *psValue;
    
    //Returns the number of values currently in an array.
    int numOfSources = CFArrayGetCount(sources);
    
    //Error in CFArrayGetCount
    if (numOfSources == 0)
    {
        NSLog(@"Error in CFArrayGetCount");
        return -1.0f;
    }
    
    //Calculating the remaining energy
    for (int i = 0 ; i < numOfSources ; i++)
    {
        //Returns a CFDictionary with readable information about the specific power source.
        pSource = IOPSGetPowerSourceDescription(blob, CFArrayGetValueAtIndex(sources, i));
        if (!pSource)
        {
            NSLog(@"Error in IOPSGetPowerSourceDescription");
            return -1.0f;
        }
        psValue = (CFStringRef)CFDictionaryGetValue(pSource, CFSTR(kIOPSNameKey));
        
        int curCapacity = 0;
        int maxCapacity = 0;
        double percent;
        
        psValue = CFDictionaryGetValue(pSource, CFSTR(kIOPSCurrentCapacityKey));
        CFNumberGetValue((CFNumberRef)psValue, kCFNumberSInt32Type, &curCapacity);
        
        psValue = CFDictionaryGetValue(pSource, CFSTR(kIOPSMaxCapacityKey));
        CFNumberGetValue((CFNumberRef)psValue, kCFNumberSInt32Type, &maxCapacity);
        
        percent = ((double)curCapacity/(double)maxCapacity * 100.0f);
        
        //NSLog(@"batteryLevel");
        //NSLog(@"%.2f", percent);
        
        return percent;
    }
    return -1.0f;
}

+ (BOOL) isBatteryCharging {
    UIDevice *device = [UIDevice currentDevice];
    device.batteryMonitoringEnabled = YES;
    if (device.batteryState == UIDeviceBatteryStateCharging){
        NSLog(@"Charging");
        return YES;
    }else{
        NSLog(@"No Charging");
        return NO;
    }
}

+ (BOOL) addSkipBackupAttributeToFileAtPath:(NSString *)aFilePath
{
    assert([[NSFileManager defaultManager] fileExistsAtPath:aFilePath]);
    
    NSError *error = nil;
    BOOL success = NO;
    
    NSString *systemVersion = [[UIDevice currentDevice] systemVersion];
    if ([systemVersion floatValue] >= 5.1f)
    {
        success = [[NSURL fileURLWithPath:aFilePath] setResourceValue:[NSNumber numberWithBool:YES]
                                                               forKey:NSURLIsExcludedFromBackupKey
                                                                error:&error];
    }
    else if ([systemVersion isEqualToString:@"5.0.1"])
    {
        const char* filePath = [aFilePath fileSystemRepresentation];
        const char* attrName = "com.apple.MobileBackup";
        u_int8_t attrValue = 1;
        
        int result = setxattr(filePath, attrName, &attrValue, sizeof(attrValue), 0, 0);
        success = (result == 0);
    }
    else
    {
        NSLog(@"Can not add 'do no back up' attribute at systems before 5.0.1");
    }
    
    if(!success)
    {
        NSLog(@"Error excluding %@ from backup %@", [aFilePath lastPathComponent], error);
    }
    
    return success;
}


@end
