using vCardLib.Models;
using vCardLib.Enums;
using vCardLib.Serializers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string twilioAccountSID = configuration["TwilioAccountSID"];
string twilioAuthToken = configuration["TwilioAuthToken"];
string serverUrl = "<ngrok URL>";

string vCardFolder = @"<folder>";

vCard vCardData = new vCard();
vCardData.FamilyName = "Campos";
vCardData.GivenName = "Néstor";
vCardData.PhoneNumbers = new List<TelephoneNumber>();
vCardData.PhoneNumbers.Add(new TelephoneNumber { Type = TelephoneNumberType.Work, Value = "<phone number>", CustomTypeName = "Main contact" });

string serializedvCardData = Serializer.Serialize(vCardData);

string vCardFileName = "ncampos.vcf";
string SavePath = Path.Combine(vCardFolder, vCardFileName);
File.WriteAllText(SavePath, serializedvCardData);


TwilioClient.Init(twilioAccountSID, twilioAuthToken);

var message = MessageResource.Create(
            body: "This is the main contact for Néstor.",
            from: new Twilio.Types.PhoneNumber("whatsapp:<from number>"),
            to: new Twilio.Types.PhoneNumber("whatsapp:<to number>"),
            mediaUrl:new List<Uri>() { new Uri(serverUrl+vCardFileName) }
        );

Console.WriteLine($"Message SID: {message.Sid}");
Console.WriteLine($"Message Status: {message.Status}");

