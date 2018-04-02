//////////////////////////////////////////////////////////////////
//
// RosShell.cpp - cpp file
//
// This file was generated using the RTX64 RTDLL Template for Visual Studio.
//
// Created: 3/7/2018 4:24:46 PM
// User: yvesd
//
//////////////////////////////////////////////////////////////////

#pragma once
//This define will deprecate all unsupported Microsoft C-runtime functions when compiled under RTSS.
//If using this define, #include <rtapi.h> should remain below all windows headers
//#define UNDER_RTSS_UNSUPPORTED_CRT_APIS

#include <SDKDDKVer.h>

//#include <stdio.h>
//#include <string.h>
//#include <ctype.h>
//#include <conio.h>
//#include <stdlib.h>
//#include <math.h>
//#include <errno.h>
#include <windows.h>
#include <tchar.h>
#include <rtapi.h>      // RTX64 APIs that can be used in real-time or Windows applications.

#ifdef UNDER_RTSS
#include <rtssapi.h>    // RTX64 APIs that can only be used in real-time applications.
#endif // UNDER_RTSS

#if defined UNDER_RTSS
  #if defined RTX64_EXPORTS
    #define RosShell_API __declspec(dllexport)
  #else
    #define RosShell_API __declspec(dllimport)
  #endif
#else
  #if defined DLL64_EXPORTS
    #define RosShell_API __declspec(dllexport)
  #else
    #define RosShell_API __declspec(dllimport)
  #endif
#endif


// Add DEFINES Here
typedef struct _Message {
	LARGE_INTEGER timestamp;
	long length;
	char* message;
} Message;

typedef void(__stdcall *SubscriberCallback)(Message message);
typedef bool(__stdcall *ServiceCallback)(Message* request, Message* response);
// This class is exported from the RosShell.dll
class RosShell_API Publisher
{
public:
	Publisher(void* ros, void* handle);
	~Publisher();
	void publish(Message message);
	void publish(char* message);
	void shutdown();
private:
	void* _ros;
	void* _hStatus;
	void* _pData;
};

class RosShell_API Subscriber
{
public:
	Subscriber(void* ros, void* handle, SubscriberCallback cb);
	~Subscriber();
	bool getLastMessage(Message* message, bool* isNew);
	void shutdown();
	void poll();
private:
	void* _ros;
	void* _hStatus;
	void* _pData;
	SubscriberCallback _callback;
	LARGE_INTEGER _lLastMessage;
	HANDLE _hPollingTimer;
};

class RosShell_API ServiceServer
{
public:
	ServiceServer(void* ros, void* handle);
	~ServiceServer();
	void shutdown();
	void proxyCall();
private:
	void* _ros;
	void* _hStatus;
	HANDLE _hServiceReq;
	HANDLE _hServiceAck;
	void* _pData;
	HANDLE _hProxy;
	Message* _pRequest;
	Message* _pResponse;
};

class RosShell_API ServiceClient
{
public:
	ServiceClient(void* ros, void* node, char* service);
	~ServiceClient();
	bool call(Message* request, Message* response, int timeout);
	bool isValid();
	bool isPersistent();
	bool exists();
	bool waitForExistence(int timeout);
	void shutdown();
	void searchForService();
private:
	void* _ros;
	void* _node;
	void* _hStatus;
	char _service[256];
	HANDLE _hServiceReq;
	HANDLE _hServiceAck;
	HANDLE _hServiceMutex;
	void* _pData;
	Message* _pRequest;
	Message* _pResponse;
};

class RosShell_API NodeHandle
{
public:
	NodeHandle(char* nodename, char* ns);
	~NodeHandle();
	Publisher* advertize(char* topic, int messageLength);
	Subscriber* subscribe(char* topic, SubscriberCallback cb);
	ServiceServer* advertizeService(char* service, ServiceCallback cb, int requestLength, int responseLength);
	ServiceClient* serviceClient(char* service);
private:
	HANDLE _hRos;
	void* _ros;
	void* _hStatus;
};

