//////////////////////////////////////////////////////////////////
//
// RtssApp2.cpp - cpp file
//
// This file was generated using the RTX64 Application Template for Visual Studio.
//
// Created: 6/30/2017 4:11:38 PM 
// User: yvesd
//
//////////////////////////////////////////////////////////////////

#include "RtssApp2.h"
#include "RosShell.h"

#define DEF_ROUND_DEGREE 3600
#define DEF_TARGET_AXIS 0

#define MAX_NODE_COUNT 8
#define MAX_TOPIC 64
#define MAX_SERVICE 64

   
typedef struct _RosPublisher {
	int Used;
	int NodeIndex;
	int IsAlive;
	int MessageLength;
	int ClientCount;
	char Name[256];
}RosPublisher;

typedef struct _RosService {
	int Used;
	int NodeIndex;
	int IsAlive;
	int RealTime;
	int	ClientCount;
	void* Location;
	char Name[256];
}RosService;

typedef struct _NodeDefinition {
	int Used;
	int Index;
	int IsAlive;
	int TopicCount;
	int ServiceCount;
	char NodeName[256];
	char NameSpace[256];
	RosPublisher Topics[MAX_TOPIC];
	RosService	Services[MAX_SERVICE];
}NodeDefinition;

typedef struct _MasterDefintion {
	int MaxNodeCount;
	int MaxTopic;
	int MaxService;
	int NodeCount;
	NodeDefinition Nodes[MAX_NODE_COUNT];
}MasterDefinition;


int _tmain(int argc, _TCHAR * argv[])
{
	RtPrintf("ROS Test App\n");
	//RtPrintf("TopicSize: %d\n", sizeof(RosPublisher));
	//RtPrintf("ServiceSize: %d\n", sizeof(RosService));
	//RtPrintf("NodeSize: %d\n", sizeof(NodeDefinition));
	//RtPrintf("MasterSize: %d\n", sizeof(MasterDefinition));
	//return 0;
	NodeHandle* Ros = new NodeHandle("RosRobotPlc", "Axel");
	if (Ros == NULL) {
		RtPrintf("Failed to create Node Handle\n");
		return -1;
	}
	RtPrintf("NodeHandle created\n");

	Subscriber* RobotPosition = NULL;
	
	while (RobotPosition == NULL) {
		RobotPosition = Ros->subscribe("PomeCrl/UR/RobotPosition", NULL);
		Sleep(100);
	}
	RtPrintf("Subscribe successfull\n");
	Sleep(1000);
	bool isNew = false;
	double Position[12] = { 0 };
	Message myPos = { 0 };
	myPos.timestamp.QuadPart = 0;
	myPos.length = 96;
	myPos.message = (char*)&Position[0];
	while (RobotPosition->getLastMessage(&myPos, &isNew) == false) {
		Sleep(100);
	}
	if (myPos.message == NULL) {
		RtPrintf("Failed to get message");
		return -1;
	}

	RtPrintf("Position Read:\n");
	RtPrintf("J1: %d\n", (int)Position[0]);
	RtPrintf("J2: %d\n", (int)Position[1]);
	RtPrintf("J3: %d\n", (int)Position[2]);
	RtPrintf("J4: %d\n", (int)Position[3]);
	RtPrintf("J5: %d\n", (int)Position[4]);
	RtPrintf("J6: %d\n", (int)Position[5]);
	RtPrintf("X: %d\n", (int)Position[6]);
	RtPrintf("Y: %d\n", (int)Position[7]);
	RtPrintf("Z: %d\n", (int)Position[8]);
	RtPrintf("A: %d\n", (int)Position[9]);
	RtPrintf("B: %d\n", (int)Position[10]);
	RtPrintf("C: %d\n", (int)Position[11]);

	ServiceClient* CommandService = Ros->serviceClient("PomeCrl/UR/MoveCommand");
	CommandService->waitForExistence(10000);
	RtPrintf("Service Online\n");

	Message request = { 0 };
	Message response = { 0 };
	int retCodes[2] = { 0 };
	response.length = 8;
	response.message = (char*)&retCodes[0];
	request.length = 804;
	char requestData[804] = { 0 };
	//double* pos = (double*)(&requestData[196]);
	//pos[0] = Position[0] + 10;
	//pos[1] = Position[1];
	//pos[2] = Position[2];
	//pos[3] = Position[3];
	//pos[4] = Position[4];
	//pos[5] = Position[5];
	*((int*)&requestData[0]) = 1;
	*((int*)&requestData[4]) = 0;
	*((int*)&requestData[8]) = 2;

	request.message = &requestData[0];

	bool commandSuccess = CommandService->call(&request, &response, 5000);
	RtPrintf("Command response: %d, %d\n", commandSuccess, retCodes[1]);


	return 0;
}
