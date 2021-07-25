
## Get institution list

**POST** /api/institution/getall

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": {}
        
    }

Response  **200 OK**

    {
        "jti": "9deddb29-338f-489e-a51c-25391fa11323",
        "audience": "Institution",
        "subject": "Institution Service",
        "expiration": "1584548543035.4",
        "data": [
            {
                "code": "ROB",
                "name": "Robinsons Malls",
                "status": false,
                "id": 1,
                "guid": "141ff168-febd-40f7-8990-07c79142171f",
                "createdBy": 0,
                "createdDate": "2020-03-18T21:34:21.6315239",
                "updatedBy": 0,
                "updatedDate": "2020-03-18T21:34:21.6315239"
            }
        ],
        "date": "1584541343035.41",
        "resultCode": "200",
        "resultMessage": "Institution has been success!."
    }

## Get branches of institution

**POST** /api/branch/get

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"institutionID":"dec8ec3c-c2d7-43a7-bee0-d840214ff8b1"
            }
    }

Response **200 OK**

    {
        "jti": "939bb071-b592-4baa-9f09-31e97954a57c",
        "audience": "Branch",
        "subject": "Branch Service",
        "expiration": "1584548490325.62",
        "data": [
            {
                "code": "Robinsons Malls Branch 1",
                "name": "Robinsons Malls Branch 1",
                "status": false,
                "merchantID": 1,
                "merchant": null,
                "institutionID": "141ff168-febd-40f7-8990-07c79142171f",
                "id": 1,
                "guid": "81baff5a-95ce-4142-9854-fff413566255",
                "createdBy": 0,
                "createdDate": "2020-03-18T21:34:23.0829406",
                "updatedBy": 0,
                "updatedDate": "2020-03-18T21:34:23.0829406"
            },
            {
                "code": "Robinsons Malls Branch 2",
                "name": "Robinsons Malls Branch 2",
                "status": false,
                "merchantID": 1,
                "merchant": null,
                "institutionID": "141ff168-febd-40f7-8990-07c79142171f",
                "id": 2,
                "guid": "e45d0fb8-aeb3-4999-98fc-6072b7ce219e",
                "createdBy": 0,
                "createdDate": "2020-03-18T21:34:23.0829406",
                "updatedBy": 0,
                "updatedDate": "2020-03-18T21:34:23.0829406"
            }
        ],
        "date": "1584541290325.63",
        "resultCode": "200",
        "resultMessage": "Branch has been success!."
    }

## Generate schedule

**POST** /api/branchschedule/generate

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"branchCode":"E45D0FB8-AEB3-4999-98FC-6072B7CE219E",
            	"personCount":30,
            	"hoursCount":2,
            	"isSenior":false,
            	"date":"2020-03-18 00:00:00",
            	"start":"2020-03-18 09:00:00",
            	"end":"2020-03-18 18:00:00"
            }
    }

Response **200 OK**

    {
        "jti": "66298c92-0f56-4dea-8165-20c231db9217",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584546421178.88",
        "data": null,
        "date": "1584539221178.89",
        "resultCode": "200",
        "resultMessage": "Branch Schedule has been success!."
    }

## **Getlist of available schedule**

**POST**  /api/branchschedule/getavailable

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"branchCode":"E45D0FB8-AEB3-4999-98FC-6072B7CE219E",
            	"date":"2020-03-18" // nullable
            }
        
    }

Response **200 OK**

    {
        "jti": "12524104-a777-4d5e-9267-9b3f4815d2f3",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584552579532.99",
        "data": [
            {
                "status": "OPEN",
                "storeOpen": "0001-01-01T00:00:00",
                "storeClose": "0001-01-01T00:00:00",
                "date": "2020-03-18T00:00:00",
                "startTime": "2020-03-18T09:00:00",
                "endTime": "2020-03-18T11:00:00",
                "maxPersonCount": 30,
                "isSenior": false,
                "remarks": "",
                "branchID": 2,
                "branch": "Robinsons Malls Branch 2",
                "institutionID": "141ff168-febd-40f7-8990-07c79142171f",
                "id": 3,
                "guid": "2d186ab0-d9d8-43ea-ad77-f9fd1a88a532",
                "createdBy": 0,
                "createdDate": "2020-03-18T21:45:15.6696672",
                "updatedBy": 0,
                "updatedDate": "2020-03-18T21:45:15.6696672"
            },
            {
                "status": "OPEN",
                "storeOpen": "0001-01-01T00:00:00",
                "storeClose": "0001-01-01T00:00:00",
                "date": "2020-03-18T00:00:00",
                "startTime": "2020-03-18T13:00:00",
                "endTime": "2020-03-18T15:00:00",
                "maxPersonCount": 30,
                "isSenior": false,
                "remarks": "",
                "branchID": 2,
                "branch": "Robinsons Malls Branch 2",
                "institutionID": "141ff168-febd-40f7-8990-07c79142171f",
                "id": 4,
                "guid": "a6705a1c-85a4-4f02-a012-b86e5add4b5e",
                "createdBy": 0,
                "createdDate": "2020-03-18T21:45:24.3941849",
                "updatedBy": 0,
                "updatedDate": "2020-03-18T21:45:24.3941849"
            },
            {
                "status": "OPEN",
                "storeOpen": "0001-01-01T00:00:00",
                "storeClose": "0001-01-01T00:00:00",
                "date": "2020-03-18T00:00:00",
                "startTime": "2020-03-18T17:00:00",
                "endTime": "2020-03-18T19:00:00",
                "maxPersonCount": 30,
                "isSenior": false,
                "remarks": "",
                "branchID": 2,
                "branch": "Robinsons Malls Branch 2",
                "institutionID": "141ff168-febd-40f7-8990-07c79142171f",
                "id": 5,
                "guid": "b0585585-7dc2-4ab6-a671-6df365703888",
                "createdBy": 0,
                "createdDate": "2020-03-18T21:45:24.413332",
                "updatedBy": 0,
                "updatedDate": "2020-03-18T21:45:24.413332"
            }
        ],
        "date": "1584545379533",
        "resultCode": "200",
        "resultMessage": "Branch Schedule has been success!."
    }

## Schedule remaining count

**POST** /api/branchschedule/remainingcount

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"scheduleCode":"93157C6F-D042-456E-8927-38C0B47D2864",
            }
        
    }

Response **200 OK**

    {
        "jti": "e2a7dd93-df76-431d-b0a3-7945d216e177",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584561124990.72",
        "data": 28,
        "date": "1584553924990.72",
        "resultCode": "200",
        "resultMessage": "Branch Schedule has been success!."
    }

## Reserve

**POST** /api/branchschedulemember/reserve

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"scheduleCode":"38004BAA-803D-41D2-9F95-1AE1C5499AE7",
            	"fullName":"JC Cabili",
            	"birthplace":"QC",
            	"birthdate":"1992-12-23",
            	"mobileNumber":"09178434865",
            	"email":"cabilijc@gmail.com",
            	"redirectURL":"http://localhost:7326/api/values"
            }
        
    }

Response **200 OK**

    {
        "jti": "d4168472-b957-4d88-aadd-e5a93413e20f",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584560916121.61",
        "data": {
            "refNumber": "",
            "isSenior": false,
            "otpExpiry": "2020-03-19T01:54:36.1850646+08:00",
            "memberID": 7,
            "member": "com.allcard.institution.models.Member",
            "branchScheduleID": 1,
            "id": 6,
            "guid": "d702b0c1-026d-4b98-a4bf-080788a07bfb",
            "createdBy": 0,
            "createdDate": "2020-03-19T01:48:36.2325416+08:00",
            "updatedBy": 0,
            "updatedDate": "2020-03-19T01:48:36.2325416+08:00"
        },
        "date": "1584553716121.61",
        "resultCode": "200",
        "resultMessage": "You will receive an SMS to verify OTP."
    }

## Confirm OTP

**POST** /api/branchschedulemember/otpconfirm

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"reserveCode":"EDF06AD4-E428-461E-82B1-BEA65832BBF9",
            	"OTP":"X5A8H6"
            }
        
    }

    {
        "jti": "0a20676f-cca1-4f7a-b05d-cdc41a8ca89d",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584626096561.58",
        "data": "edf06ad4-e428-461e-82b1-bea65832bbf9", // This is the reference number
        "date": "1584618896561.59",
        "resultCode": "200",
        "resultMessage": "Success!"
    }

## Scan

**POST** /api/branchschedulemember/scan

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"reserveCode":"D702B0C1-026D-4B98-A4BF-080788A07BFB",
    					"BranchCode":"279cb69d-744f-48ac-9f41-abf4bab4735c"
            }
        
    }

Response **201 Failed, 200 OK**

    {
        "jti": "279cb69d-744f-48ac-9f41-abf4bab4735c",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584562591248.35",
        "data": null,
        "date": "1584555391248.38",
        "resultCode": "201",
        "resultMessage": "Member schedule has already ended. 3/18/2020 9:00:00 AM to 3/18/2020 11:00:00 AM!."
    }

## Authenticate

**POST** /api/auth/login

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": {
        	"username":"guard1",
        	"password":"p@ssw0rd"
        }
        
    }

Response **200 OK**

    {
        "jti": "0e9e2752-77ec-442d-8b14-47977f3a94d8",
        "audience": null,
        "subject": null,
        "expiration": "1584617218587.02",
        "data": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1Y29kZSI6ImUwNDVhYmZhLTAyMWMtNDZjZi1hZmE2LTljZmQ1ZmIwZDI5NSIsInIiOiIzIiwiYmNvZGUiOiJhMmJmYmIxZC1kYmE1LTQxMmItYjlhZC03NzIwMTZiZjYwNGQiLCJuYmYiOjE1ODQ2MTAwMjIsImV4cCI6MTU4NTIxNDgyMiwiaWF0IjoxNTg0NjEwMDIyfQ.zSlLfjP1RmclHyvyhxUNNrmZkPQ9L9K3SR2BPb5YlJg",
        "date": "1584610018588.03",
        "resultCode": "200",
        "resultMessage": " has been success!."
    }

Token value:

    {
      "ucode": "e045abfa-021c-46cf-afa6-9cfd5fb0d295", // usercode
      "r": "3", // role
      "bcode": "a2bfbb1d-dba5-412b-b9ad-772016bf604d", // branch code
      "nbf": 1584610022,
      "exp": 1585214822, // Token expiration.
      "iat": 1584610022
    }

## Cancel Schedule

**POST** /api/branchschedule/cancel

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"scheduleCode":"A2BFBB1D-DBA5-412B-B9AD-772016BF604D", // can cancel by code
            	"date":"2020-03-20", // by date
            }
    }

Response **200 OK**

    {
        "jti": "a06328b5-c6bd-4e12-8c0e-44cd3bd17a64",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584677730538.74",
        "data": null,
        "date": "1584670530538.95",
        "resultCode": "201",
        "resultMessage": "No schedule available to be cancel."
    }

## Get Branch Schedule

**POST**  /api/branchschedule/myschedule

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"branchCode":"A2BFBB1D-DBA5-412B-B9AD-772016BF604D",
            	"page":4, // page of the schedule.
            	"row":5 // total row count to show.
            }
    }

Respone **200 OK**

    {
        "jti": "ef8001b1-a7f9-4f97-b4f3-348c90d2d8d8",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584680799870.65",
        "data": {
            "list": [
                {
                    "status": "CANCELLED",
                    "date": "2020-03-19T00:00:00",
                    "startTime": "2020-03-19T13:00:00",
                    "endTime": "2020-03-19T15:00:00",
                    "maxPersonCount": 30,
                    "isSenior": false,
                    "availableSlot": 0,
                    "remarks": "",
                    "branchID": 1,
                    "branch": "Rockwell makati",
                    "institutionID": "434a7eff-cefd-455d-863b-5d6c36d9ceb6",
                    "id": 2,
                    "guid": "dfc57ad3-6bf9-4ec3-b94f-af900dc14d00",
                    "createdBy": 0,
                    "createdDate": "2020-03-19T19:49:41.9644989",
                    "updatedBy": 0,
                    "updatedDate": "2020-03-19T19:49:41.9644989"
                },
                {
                    "status": "CANCELLED",
                    "date": "2020-03-19T00:00:00",
                    "startTime": "2020-03-19T17:00:00",
                    "endTime": "2020-03-19T19:00:00",
                    "maxPersonCount": 30,
                    "isSenior": false,
                    "availableSlot": 0,
                    "remarks": "",
                    "branchID": 1,
                    "branch": "Rockwell makati",
                    "institutionID": "434a7eff-cefd-455d-863b-5d6c36d9ceb6",
                    "id": 3,
                    "guid": "ef052a45-f08f-4c1c-b855-b91ec6de8261",
                    "createdBy": 0,
                    "createdDate": "2020-03-19T19:49:41.9930846",
                    "updatedBy": 0,
                    "updatedDate": "2020-03-19T19:49:41.9930846"
                },
                {
                    "status": "CANCELLED",
                    "date": "2020-03-19T00:00:00",
                    "startTime": "2020-03-19T20:00:00",
                    "endTime": "2020-03-19T22:00:00",
                    "maxPersonCount": 30,
                    "isSenior": false,
                    "availableSlot": 0,
                    "remarks": "",
                    "branchID": 1,
                    "branch": "Rockwell makati",
                    "institutionID": "434a7eff-cefd-455d-863b-5d6c36d9ceb6",
                    "id": 4,
                    "guid": "0996c0d7-62c4-4029-8fd7-e5695eeadcd8",
                    "createdBy": 0,
                    "createdDate": "2020-03-19T20:01:33.8518156",
                    "updatedBy": 0,
                    "updatedDate": "2020-03-19T20:01:33.8518156"
                }
            ],
            "totalCount": 18,
            "pageCount": 4
        },
        "date": "1584673599870.65",
        "resultCode": "200",
        "resultMessage": "Branch Schedule has been success!."
    }

## Board

**POST** /api/branchschedule/board

    {
        "jti": "e560bd47-239a-47bc-96f1-eba33c41556c",
        "audience": "Institution",
        "subject": "Institution Service",
        "data": 
            {
            	"branchCode":"A2BFBB1D-DBA5-412B-B9AD-772016BF604D",
            	"date":'2020-03-20',
            }
    }

Response **200 OK**

    {
        "jti": "f6ff2587-96f3-47f0-b641-5576b7402eb8",
        "audience": "Branch Schedule",
        "subject": "Branch Schedule",
        "expiration": "1584682585047.93",
        "data": [
            {
                "status": "OPEN",
                "scheduleCode": "d5869fe8-42dc-4c50-b7e7-e72771f8a433",
                "startTime": "2020-03-20T09:00:00",
                "endTime": "2020-03-20T10:59:59",
                "totalSlots": 30,
                "availableSlot": 30,
                "totalVerified": 0
            },
            {
                "status": "OPEN",
                "scheduleCode": "ddf3528a-360b-48f1-b571-bebfa4917b4d",
                "startTime": "2020-03-20T11:00:00",
                "endTime": "2020-03-20T12:59:59",
                "totalSlots": 30,
                "availableSlot": 30,
                "totalVerified": 0
            },
            {
                "status": "OPEN",
                "scheduleCode": "b9cbba68-0558-433f-957b-0759ba5cd798",
                "startTime": "2020-03-20T13:00:00",
                "endTime": "2020-03-20T14:59:59",
                "totalSlots": 30,
                "availableSlot": 30,
                "totalVerified": 0
            },
            {
                "status": "OPEN",
                "scheduleCode": "0889fcf3-5191-40ca-9d1d-b53a1b6865f6",
                "startTime": "2020-03-20T15:00:00",
                "endTime": "2020-03-20T16:59:59",
                "totalSlots": 30,
                "availableSlot": 30,
                "totalVerified": 0
            },
            {
                "status": "OPEN",
                "scheduleCode": "90b8c981-e8bd-4051-9572-8db97043e06a",
                "startTime": "2020-03-20T17:00:00",
                "endTime": "2020-03-20T18:00:00",
                "totalSlots": 30,
                "availableSlot": 30,
                "totalVerified": 0
            }
        ],
        "date": "1584675385048.19",
        "resultCode": "200",
        "resultMessage": "Branch Schedule has been success!."
    }