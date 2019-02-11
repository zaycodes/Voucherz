using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Service;

namespace VoucherService.MQ
{
    public class Subscribers:BackgroundService
    {
        private IVoucherService baseVoucherService;
        private ILogger<Subscribers> _logger;
        private ConnectionFactory connection;

        public Subscribers(IVoucherService baseVoucherService, ILogger<Subscribers> _logger,ConnectionFactory connection)
        {
            this.baseVoucherService = baseVoucherService;
            this._logger = _logger;
            this.connection = connection;
        }

        public void CodeReceiver()
        {


            var conn = connection.CreateConnection();
            var channel = conn.CreateModel();

            channel.QueueDeclare("gift-one", true, false, false, null);
            channel.QueueDeclare("gift-two", true, false, false, null);
            channel.QueueDeclare("gift-three", true, false, false, null);

            var consumerDiscount = new EventingBasicConsumer(channel);
            consumerDiscount.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var deserialized = JsonConvert.DeserializeObject<Gift>(message);
                _logger.LogDebug("Received Discount Object:", deserialized.ToString());
                baseVoucherService.UpdateGiftVoucherAmount(deserialized.Code, deserialized.GiftBalance);
                _logger.LogDebug("Successful Update of Discount voucher:", deserialized.Code);
            };

            channel.BasicConsume(queue: "gift-one",
                                 autoAck: true,
                                 consumer: consumerDiscount);


            var consumerGift = new EventingBasicConsumer(channel);
            consumerGift.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var deserialized = JsonConvert.DeserializeObject<Gift>(message);
                _logger.LogDebug("Received Gift Object{0} {1}", deserialized.Code, deserialized.GiftBalance);
                baseVoucherService.UpdateGiftVoucherAmount(deserialized.Code, deserialized.GiftBalance);
                _logger.LogDebug("Successful Update of GiftVoucher amount for voucher {0}", deserialized.Code);
            };
            channel.BasicConsume(queue: "gift-two",
                                 autoAck: true,
                                 consumer: consumerGift);

            var consumerValue = new EventingBasicConsumer(channel);
            consumerValue.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var deserialized = JsonConvert.DeserializeObject<Value>(message);
                _logger.LogDebug("Received Value Object:", deserialized.Code, deserialized.ValueAmount);
                baseVoucherService.ActivateOrDeactivateVoucher(deserialized.Code);
                _logger.LogDebug("Successful Update of ValueVoucher:", deserialized.Code);
            };

            channel.BasicConsume(queue: "gift-three",
                                 autoAck: true,
                                 consumer: consumerValue);


        }

       
        
     
         protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Listeners Up and Running!!!!");    
            return Task.Run(()=>CodeReceiver());
        }

        

        
    }
}
