using EtlService;
using EtlService.Configuration;

Directory.SetCurrentDirectory("./../../../../");

IConfiguration configuration = new Configuration("config.json");
configuration.ValidateConfiguration();

var app = new ConsoleApplication(configuration);
app.Run();