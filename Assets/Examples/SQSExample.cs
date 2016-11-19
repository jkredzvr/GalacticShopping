//// Copyright 2014-2015 Amazon.com, 
//// Inc. or its affiliates. All Rights Reserved.
//// 
//// Licensed under the Amazon Software License (the "License"). 
//// You may not use this file except in compliance with the 
//// License. A copy of the License is located at
//// 
////     http://aws.amazon.com/asl/
//// 
//// or in the "license" file accompanying this file. This file is 
//// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
//// CONDITIONS OF ANY KIND, express or implied. See the License 
//// for the specific language governing permissions and 
//// limitations under the License.
////


/////Currently this script polls the SQS queue list.  SQS messages should be deleted or they will be reread again.


//using UnityEngine;
//using System.Collections;
//using Amazon;
//using Amazon.Runtime;
//using Amazon.CognitoIdentity;
//using Amazon.SQS;
//using UnityEngine.UI;

////namespace AWSSDK.Examples
////{
//    public class SQSExample : MonoBehaviour
//{
//    //identity pool id for cognito credentials

//    //CHANGE THIS
//    private string IdentityPoolId = "us-east-1:16278071-7e14-4927-911a-f02dcaf576e6";
//    public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;

//    private RegionEndpoint _CognitoIdentityRegion
//    {
//        get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
//    }

//    public string SQSRegion = RegionEndpoint.USEast1.SystemName;

//    private RegionEndpoint _SQSRegion
//    {
//        get { return RegionEndpoint.GetBySystemName(SQSRegion); }
//    }


//    #region SQS Queue Info
//    //name of the queue you want to create

//    //CHANGE THIS - SQS Queue
//    private string QueueName = "colorexpert";
//    private AWSCredentials _credentials;
//    private AWSCredentials Credentials
//    {
//        get
//        {
//            if (_credentials == null)
//                _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
//            return _credentials;
//        }
//    }

//    private IAmazonSQS _sqsClient;
//    private IAmazonSQS SqsClient
//    {
//        get
//        {
//            if (_sqsClient == null)
//                _sqsClient = new AmazonSQSClient(Credentials, _SQSRegion);
//            return _sqsClient;
//        }
//    }

//    //CHANGETHIS entere SQS Queue URL
//    private string queueUrl = "https://sqs.us-east-1.amazonaws.com/522674837128/colorexpert" + "/?Action=SetQueueAttributes&Attribute.Name=ReceiveMessageWaitTimeSeconds&Attribute.Value=20";
//    #endregion


//    #region Unity Alexa SQS UI
//    public Button CreateQueue;
//    public Button SendMessage;
//    public Button RetrieveMessage;
//    public Button DeleteQueue;
//    public InputField Message;
//    #endregion

  
//    // Use this for initialization
//    void Start()
//    {
//        UnityInitializer.AttachToGameObject(this.gameObject);
//        //Unity event fired on click.... listen to them
//        CreateQueue.onClick.AddListener(CreateQueueListener);
//        SendMessage.onClick.AddListener(SendMessageListener);
//        RetrieveMessage.onClick.AddListener(RetrieveMessageListener);
//        DeleteQueue.onClick.AddListener(DeleteQueueListener);

//        //Automatic polling of Retreive Message call
//        StartCoroutine(RepeatRetrieveMessage(0.1F));
//    }

//    /// <summary>
//    /// ??????
//    /// </summary>
//    private void CreateQueueListener()
//    {
//        //            SqsClient.CreateQueueAsync(QueueName, (result) =>
//        //            {
//        //                if (result.Exception == null)
//        //                {
//        //                    Debug.Log(@"Queue Created");
//        //                    queueUrl = result.Response.QueueUrl;
//        //                }
//        //                else
//        //                {
//        //                    Debug.LogException(result.Exception);
//        //                }
//        //            });
//    }

//    /// <summary>
//    /// ????
//    /// </summary>
//    private void DeleteQueueListener()
//    {
//        //            if (!string.IsNullOrEmpty(queueUrl))
//        //            {
//        //                SqsClient.DeleteQueueAsync(queueUrl, (result) =>
//        //                {
//        //                    if (result.Exception == null)
//        //                    {
//        //                       Debug.Log(@"Queue Deleted");
//        //                    }
//        //                    else
//        //                    {
//        //                        Debug.LogException(result.Exception);
//        //                    }
//        //                });
//        //            }
//        //            else
//        //            {
//        //                Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
//        //            }
//    }

//    /// <summary>
//    /// Seems like it reads the text property of the Message component and sends it SQS
//    /// </summary>
//    private void SendMessageListener()
//    {
//        if (!string.IsNullOrEmpty(queueUrl))
//        {
//            var message = Message.text;
//            if (string.IsNullOrEmpty(message))
//            {
//                Debug.Log("No Message to send");
//                return;
//            }

//            SqsClient.SendMessageAsync(queueUrl, message, (result) =>
//            {
//                if (result.Exception == null)
//                {
//                    Debug.Log("Message Sent");
//                }
//                else
//                {
//                    Debug.LogException(result.Exception);
//                }
//            });
//        }
//        else
//        {
//            Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
//        }
//    }

//    /// <summary>
//    /// Constant SQS Message Polling Coroutine, process message, and then maybe delete the message
//    /// </summary>
//    /// <param name="waitTime"></param>
//    /// <returns></returns>
//    IEnumerator RepeatRetrieveMessage(float waitTime)
//    {
//        bool checkSQS = true;
//        while (checkSQS)
//        {
//            yield return new WaitForSeconds(waitTime);

//            if (!string.IsNullOrEmpty(queueUrl))
//            {
//                SqsClient.ReceiveMessageAsync(queueUrl, (result) =>
//                {
//                    if (result.Exception == null)
//                    {
//                        //Read the message
//                        var messages = result.Response.Messages;
//                        messages.ForEach(m =>
//                        {
//                            Debug.Log(@"Message Id  = " + m.MessageId);
//                            Debug.Log(@"Mesage = " + m.Body);

//                            //Process the message
//                            //[do your thing here]

//                            //Delete the message
//                            var delRequest = new Amazon.SQS.Model.DeleteMessageRequest
//                            {
//                                QueueUrl = queueUrl,
//                                ReceiptHandle = m.ReceiptHandle

//                            };

//                            SqsClient.DeleteMessageAsync(delRequest, (delResult) =>
//                            {
//                                if (delResult.Exception == null)
//                                {
//                                }
//                                else
//                                {
//                                }
//                            });
//                        });

//                    }
//                    else
//                    {
//                        Debug.LogException(result.Exception);
//                    }
//                });
//            }
//            else
//            {
//                Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
//            }

//            //Debug.Log (".");
//        }
//    }

//    /// <summary>
//    /// ???
//    /// </summary>
//    private void RetrieveMessageListener()
//    {
//        StartCoroutine(RepeatRetrieveMessage(0.1F));
//    }

//}

////}
