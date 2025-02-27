﻿using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace BikeRental.MessageQueue.Consumer;

public class SqsConsumer : IConsumer
{
    private readonly AmazonSQSClient _amazonSqs;

    public SqsConsumer()
    {
        var basicCredentials = new BasicAWSCredentials("AKIA2JUZUHJXQBX37EUR", 
            "bP0ROTQSIv8nJr1P+OZ91duCzOhElC9ud2qG2db0");
        _amazonSqs = new AmazonSQSClient(basicCredentials, RegionEndpoint.USEast1);
    }
    
    public async Task<List<Message>> ReceiveMessages(string queue)
    {
        var request = new ReceiveMessageRequest  
        {  
            QueueUrl = queue,  
            MaxNumberOfMessages = 10,  
            WaitTimeSeconds = 5  
        };
        
        var result = await _amazonSqs.ReceiveMessageAsync(request);
        if (result.HttpStatusCode != HttpStatusCode.OK) throw new Exception("Error when pull messages from queue");

        if (result.Messages.Any())
        {
            await _amazonSqs.DeleteMessageBatchAsync(queue, result.Messages.Select(x => 
                new DeleteMessageBatchRequestEntry
                {
                    Id = x.MessageId,
                    ReceiptHandle = x.ReceiptHandle
                }).ToList());
        }
        
        return result.Messages;
    }
}