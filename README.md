# SimpleDNS
## A simple but comprehensive DNS library for C#

### What is SimpleDNS?

SimpleDNS is an open source library which queries a DNS server of your choice and maps the received values to an object called DnsPacket. It was designed with ease in mind as well as giving the developer the option to drill down into results similar to how a DNS packet is viewed in a tool like Wireshark. It's been built to the standard proposed in the official RFC and the code has many references to certain sections of the RFC to aid other developers who wish to work on the project.
What makes SimpleDNS different to other DNS libraries?

You can work with a DNS packet (Send/Receive) using OOP and the DNS results like the Flags in the header (Response, Opcode, Authoritative etc...) are all shown as immutable properties. Basically, you don't just get the result, you get the whole packet parsed into an object.

### Okay...so how do I use it?

It's simple, you can either create a DNS packet manually used for sending which will then return a new object instance with results, or you can use the simple custom SimpleDNS model which in the background creates a DNSPacket using your values from the SimpleDNS model.

#### SimpleDNS Model Example:

```
//Create your question - (Hostname,QueryType)
var question = new Question("i.stack.imgur.com", QType.A);

//Create your SimpleDnsPacket and pass to the Util method Kitson.Dns.Query 
var result = Query.SimpleDnsAsync(new SimpleDnsPacket(question, IPAddress.Parse("8.8.8.8")));
```

#### Core DNS Model Example:

```
//DNS Header
UInt16 transactionId = 200;
IParameters parameters = new SendParameters(OPCode.Standard, true, false);
UInt16 questionsCount = 1;
UInt16 answersCounts = 0;
UInt16 authority = 0;
UInt16 additional = 0;

//DNS Question
var question = new Question("i.stack.imgur.com", QType.A);

//DNS Header Construction
IDnsHeader header = new DnsHeader(transactionId,parameters,questionsCount, answersCounts,authority, additional);

//SendDnsPacket Construction
SendDnsPacket packet = new SendDnsPacket(header, question);

//Finally...Send your packet to the chosen DNS Server
var result = Query.Dns(packet, IPAddress.Parse("8.8.8.8"));
```

And the results returned from the query above can be seen below...

##### Header Result:
![Header](https://www.kitson-online.co.uk/wp-content/uploads/2018/04/resource1.png)

##### CName Record Result:
![CName](https://www.kitson-online.co.uk/wp-content/uploads/2018/04/resource2.png)

##### A Record Result:
![ARecord](https://www.kitson-online.co.uk/wp-content/uploads/2018/04/resource3.png)

