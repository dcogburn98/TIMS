using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    [DataContract]
    public class MailMessage
    {
        [DataMember]
        public Guid ID;
        [DataMember]
        public string Subject;
        [DataMember]
        public string MessageBody;
        [DataMember]
        public bool Read;
        [DataMember]
        public DateTime SendDate;
        [DataMember]
        public DateTime ReadDate;
        [DataMember]
        public Employee Sender;
        [DataMember]
        public Employee Recipient;

        public MailMessage(string subject, string body, Employee sender, Employee recipient)
        {
            ID = Guid.NewGuid();
            ReadDate = DateTime.MinValue;
            Read = false;
            Subject = subject;
            MessageBody = body;
            Sender = sender;
            Recipient = recipient;
        }

        public static List<MailMessage> MailMerge(List<Employee> recipients, MailMessage original)
        {
            List<MailMessage> messages = new List<MailMessage>();
            foreach (Employee e in recipients)
            {
                MailMessage msg = new MailMessage(original.Subject, original.MessageBody, original.Sender, original.Recipient);
                msg.Recipient = e;
                messages.Add(msg);
            }
            if (messages.FirstOrDefault(el => el.Recipient.employeeNumber == original.Recipient.employeeNumber) == default(MailMessage))
            {
                messages.Add(original);
            }
            return messages;
        }
    }
}
