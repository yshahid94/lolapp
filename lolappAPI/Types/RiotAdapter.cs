using lolappAPI.Repository;

namespace lolappAPI.Types
{
    public class RiotAdapter
    {
        private RiotRestAPI riotRestAPI = null;

        //public void SendOutboundMessages(RiotOutboundMessage message)
        //{
        //    RiotInboundMessage responses = new RiotInboundMessage;

        //    for (int i = 0; i < messages.Messages.Count; i++)
        //    {
        //        var gammaOrder = messages.Messages[i];

        //        // Map message to an order type
        //        var gammaOrderType = MapMessageToOrder(gammaOrder.GetType().Name);

        //        // Transform the order
        //        var transformedMessage = gammaOrderType.Transform(gammaOrder);

        //        //Pass the transform to the send message
        //        var response = SendMessage(transformedMessage);

        //        //Handle response            
        //        var gammaMessageResult = gammaOrderType.HandleResponse(transformedMessage.VoiceOrderID, response, this.restAPI.Error);

        //        // Try order again depending on error message
        //        if (gammaMessageResult.IsError && gammaOrderType.ReSubmitOrder(gammaMessageResult.ExternalMessage, gammaOrder, out gammaOrder))
        //        {
        //            // Transform fixed version of order
        //            transformedMessage = gammaOrderType.Transform(gammaOrder);
        //            response = SendMessage(transformedMessage);
        //            gammaMessageResult = gammaOrderType.HandleResponse(transformedMessage.VoiceOrderID, response, this.restAPI.Error);
        //        }

        //        responses.Add(response);
        //        sentMessages.Add(gammaMessageResult);
        //        anyError = anyError || gammaMessageResult.IsError; //If any message in the list has an error, we keep this as true

        //    }


        //    return null;
        //}

        ////public InboundMessage_Type TransformInboundMessage(string xmlMessage, string messageName)
        ////{
        ////    throw new NotImplementedException();
        ////}


        //#region Private Helpers

        //private RiotOutboundMessage MapMessageToOrder(string messageType)
        //{
        //    switch (messageType)
        //    {
        //        case "GetSummonerByNameOutboundMessage":
        //            return new GetSummonerByNameOutboundMessage();
        //        default:
        //            return null;
        //    }
        //}

        //private GC.Gamma_InboundMessage SendMessage(OutboundGammaMessage message)
        //{
        //    restAPI = this.InitializeAPI(message);

        //    GC.Gamma_InboundMessage response = null;

        //    if (message.MessageType == RestAPIHelper.MessageType_Enum.GET)
        //    {

        //        var isGnpOrderSummary = message.APIType == OutboundGammaMessage.APIType_Enum.GNP && message.ResponseType == typeof(GC.GetSingleOrder_InboundMessage);
        //        if (isGnpOrderSummary)
        //        {
        //            response = restAPI.Get<GC.GetGNPSingleOrder_InboundMessage>(message.ParamString, message.OrderURL, message.ResponseType);
        //        }
        //        else
        //        {
        //            response = restAPI.Get<GC.Gamma_InboundMessage>(message.ParamString, message.OrderURL, message.ResponseType);
        //        }

        //    }
        //    else if (message.MessageType == RestAPIHelper.MessageType_Enum.POST)
        //    {
        //        response = restAPI.Post(message.Body, message.OrderURL);
        //    }


        //    return response;
        //}


        //#endregion

    }
}
