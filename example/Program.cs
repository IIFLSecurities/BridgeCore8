using BridgeNet;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePOC
{
    internal class Program
    {
        public static int count=0;
        public static async Task Main(string[] args)
        {
            IConnect CS = Connect.Instance;
            ConnectReq connectReq = new ConnectReq();

            //Event handlers
            CS.OnDisconnect += (resultCode, message) =>
            {
                Console.WriteLine("Disconnected with result code: " + resultCode + " and message: " + message);
            };

            //On error event handler
            CS.OnError += (errorCode, errMsg) =>
            {
                Console.WriteLine("Error code: " + errorCode + " and message: " + errMsg);
            };

            //On feed data received event handler
            CS.OnFeedDataReceived += (data, topic) =>
            {
                
                MWBOCombined markertFeed = BuildStruct<MWBOCombined>(data);
                count++;
                //Console.WriteLine("Marketfeed received for topic: " + topic);
                //Console.WriteLine("LTP : " + markertFeed.Ltp);
            };


            //On open interest data received event handler
            CS.OnOpenInterestDataReceived += (data, topic) =>
            {
                OpenInterestData openInterestData = BuildStruct<OpenInterestData>(data);
                Console.WriteLine("Open interest data received for topic: " + topic);
                Console.WriteLine("Open Interest: " + openInterestData.OpenInterest);
            };

            //On market status data received event handler
            CS.OnMarketStatusDataReceived += (data, topic) =>
            {
                MarketStatusData marketStatusData = BuildStruct<MarketStatusData>(data);
                Console.WriteLine("Market status data received for topic: " + topic);
                Console.WriteLine("Market status code: " + marketStatusData.MarketStatusCode);
            };

            //On LPP data received event handler
            CS.OnLppDataReceived += (data, topic) =>
            {
                LppData lppData = BuildStruct<LppData>(data);
                Console.WriteLine("LPP data received for topic: " + topic);
                Console.WriteLine("LPP High: " + lppData.LppHigh);
            };

            //On high 52 week data received event handler
            CS.OnHigh52WeekDataReceived += (data, topic) =>
            {
                High52WeekData high52WeekData = BuildStruct<High52WeekData>(data);
                Console.WriteLine("High 52 week data received for topic: " + topic);
                Console.WriteLine("High 52 week: " + high52WeekData.High52Week);
            };

            //On low 52 week data received event handler
            CS.OnLow52WeekDataReceived += (data, topic) =>
            {
                Low52WeekData low52WeekData = BuildStruct<Low52WeekData>(data);
                Console.WriteLine("Low 52 week data received for topic: " + topic);
                Console.WriteLine("Low 52 week: " + low52WeekData.Low52Week);
            };

            //On upper circuit data received event handler
            CS.OnUpperCircuitDataReceived += (data, topic) =>
            {
                UpperCircuitData upperCircuitData = BuildStruct<UpperCircuitData>(data);
                Console.WriteLine("Upper circuit data received for topic: " + topic);
                Console.WriteLine("Upper circuit: " + upperCircuitData.UpperCircuit);
            };

            //On lower circuit data received event handler
            CS.OnLowerCircuitDataReceived += (data, topic) =>
            {
                LowerCircuitData lowerCircuitData = BuildStruct<LowerCircuitData>(data);
                Console.WriteLine("Lower circuit data received for topic: " + topic);
                Console.WriteLine("Lower circuit: " + lowerCircuitData.LowerCircuit);
            };

            //On order updates received event handler
            CS.OnOrderUpdatesReceived += (data, topic) =>
            {
                Console.WriteLine("Order updates received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            //On trade updates received event handler
            CS.OnTradeUpdatesReceived += (data, topic) =>
            {
                Console.WriteLine("Trade updates received for topic: " + topic + " and data: " + Encoding.UTF8.GetString(data));
            };

            Task.Run(()=>Program.printCount());

            connectReq.host = "bridge.iiflcapital.com";
            connectReq.port = 9906;
            connectReq.token = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICIxVks4TEhlRnRvSmp6YWk1RmJlSGNPbDI3ekpGanBScTE2Vmt4eGJBZ0ZjIn0.eyJleHAiOjE3NTgzNDE3MzEsImlhdCI6MTc0Mjc4OTczMSwianRpIjoiNzFhNjYzMzgtODU0OC00MDcxLTgxMDctZmRmNjdjMjBlYWIyIiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5paWZsc2VjdXJpdGllcy5jb20vcmVhbG1zL0lJRkwiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiNDk4NTU2NTAtMDcyNS00ZGUzLWFlMmQtN2ExMGYxYzIwODI4IiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiSUlGTCIsInNpZCI6ImQwZDM1MTZhLTkxOWUtNGU5OC1iZmJmLTI3ZTg3OTJiZjI3MSIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovLzEwLjEyNS42OC4xNDQ6ODA4MC8iXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbImRlZmF1bHQtcm9sZXMtaWlmbCIsIm9mZmxpbmVfYWNjZXNzIiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJJSUZMIjp7InJvbGVzIjpbIkdVRVNUX1VTRVIiLCJBQ1RJVkVfVVNFUiJdfSwiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJvcGVuaWQgZW1haWwgcHJvZmlsZSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJ1Y2MiOiI5MzA4MDA0OCIsInNvbGFjZV9ncm91cCI6IlNVQlNDUklCRVJfQ0xJRU5UIiwibmFtZSI6IlNBVEhFRVNIIEtVTUFSIEdPUElTRVRUWSBOQSIsInByZWZlcnJlZF91c2VybmFtZSI6IjkzMDgwMDQ4IiwiZ2l2ZW5fbmFtZSI6IlNBVEhFRVNIIEtVTUFSIEdPUElTRVRUWSIsImZhbWlseV9uYW1lIjoiTkEiLCJlbWFpbCI6InNhdGlzaGt1bWFyZ29waXNldHR5MTExQGdtYWlsLmNvbSJ9.8t9A2KBDWb5sqi8M9ObhThKea0VCPrsC3E85iK472ff0KDVpXt9UY9nTIHie9Q4oJeIKzZraXwfZodWlkWFNPVzX2uH4DZjRVwTkxmaMFO3f4vMy4sBZPTprTmvB89qHlmLfEamnoB6C0wfm507QyAQC4PTuek4qVFdb8lvPcOEm0E9yGvzAbs64RMx8J1M8aql3OrfKqPdTfMb3GE3Qh4drBjRWBBWLhpbcwJmyYGFs7Wc-UXKH1xTjl0idyxCyyxMgWp9V4YlACTovoail5_a8rpQL03GzsVt5E4xwwVZXYPHLd2bvmax6cITEomzAC6rxpd4jgLf5gd4CiyY3RA";
            string JsonReq = JsonConvert.SerializeObject(connectReq);

            //Connect to the host
            string connectRes = await CS.ConnectHost(JsonReq);
            Console.WriteLine(connectRes);



            //Subscribe to the feed
            SubscribeReq subscribeReq = new SubscribeReq(new List<string> { "nsefo/35001", "nsefo/46986", "nsefo/51118", "nsefo/51122", "nsefo/51124", "nsefo/51399", "nsefo/51473", "nsefo/51475", "nsefo/53846", "nsefo/53976", "nsefo/54719", "nsefo/54727", "nsefo/55060", "nsefo/55102", "nsefo/55197", "nsefo/56528", "nsefo/56870", "nsefo/56872", "nsefo/56880", "nsefo/56882", "nsefo/56888", "nsefo/58782", "nsefo/64578", "nsefo/64582", "nsefo/64555", "nsefo/46787", "nsefo/64561", "nsefo/46943", "nsefo/64563", "nsefo/46953", "nsefo/46987", "nsefo/46998", "nsefo/47026", "nsefo/47039", "nsefo/47041", "nsefo/48191", "nsefo/48243", "nsefo/64565", "nsefo/48346", "nsefo/48348", "nsefo/49835", "nsefo/51119", "nsefo/51123", "nsefo/51127", "nsefo/51400", "nsefo/51474", "nsefo/51476", "nsefo/53847", "nsefo/53979", "nsefo/54720", "nsefo/64567", "nsefo/48695", "nsefo/51120", "nsefo/51459", "nsefo/51477", "nsefo/54721", "nsefo/54723", "nsefo/64566", "nsefo/54725", "nsefo/54819", "nsefo/55339", "nsefo/55656", "nsefo/56526", "nsefo/56534", "nsefo/56542", "nsefo/56546", "nsefo/56568", "nsefo/56572", "nsefo/56866", "nsefo/56894", "nsefo/64572", "nsefo/46795", "nsefo/46801", "nsefo/46803", "nsefo/46855", "nsefo/46882", "nsefo/46893", "nsefo/46909", "nsefo/46959", "nsefo/46975", "nsefo/48212", "nsefo/48350", "nsefo/48696", "nsefo/51121", "nsefo/51460", "nsefo/51478", "nsefo/64564", "nsefo/56566", "nsefo/56868", "nsefo/56884", "nsefo/56886", "nsefo/58786", "nsefo/58793", "nsefo/64553", "nsefo/46901", "nsefo/46989", "nsefo/47021", "nsefo/48246", "nsefo/48347", "nsefo/64570", "nsefo/59501", "nsefo/59517", "nsefo/59519", "nsefo/59521", "nsefo/59523", "nsefo/59541", "nsefo/59546", "nsefo/59548", "nsefo/59552", "nsefo/61098", "nsefo/59683", "nsefo/61110", "nsefo/59789", "nsefo/61138", "nsefo/61066", "nsefo/61073", "nsefo/61076", "nsefo/59223", "nsefo/61083", "nsefo/59348", "nsefo/59388", "nsefo/59395", "nsefo/59408", "nsefo/59439", "nsefo/59457", "nsefo/59476", "nsefo/59490", "nsefo/59496", "nsefo/59498", "nsefo/59500", "nsefo/59518", "nsefo/59520", "nsefo/59524", "nsefo/59542", "nsefo/59547", "nsefo/59483", "nsefo/61092", "nsefo/59550", "nsefo/59554", "nsefo/59567", "nsefo/59569", "nsefo/59589", "nsefo/59595", "nsefo/59630", "nsefo/61104", "nsefo/59703", "nsefo/59783", "nsefo/59805", "nsefo/59831", "nsefo/59881", "nsefo/59935", "nsefo/61064", "nsefo/59139", "nsefo/59203", "nsefo/59297", "nsefo/61085", "nsefo/59429", "nsefo/59435", "nsefo/59437", "nsefo/61091", "nsefo/59468", "nsefo/59484", "nsefo/59494", "nsefo/61097", "nsefo/59502", "nsefo/59522", "nsefo/59551", "nsefo/59599", "nsefo/59625", "nsefo/59677", "nsefo/59123", "nsefo/59167", "nsefo/59189", "nsefo/59243", "nsefo/59265", "nsefo/59327", "nsefo/59358", "nsefo/59425", "nsefo/59453", "nsefo/59452", "nsefo/59459", "nsefo/59499", "nsefo/59591", "nsefo/59609", "nsefo/59611", "nsefo/59665", "nsefo/59785", "nsefo/59803", "nsefo/59042", "nsefo/59044", "nsefo/59169", "nsefo/59385", "nsefo/59460", "nsefo/59549", "nsefo/59613", "nsefo/59735", "nsefo/59362", "nsefo/59628", "nsefo/59647", "nsefo/59115", "bsefo/863654", "bsefo/863694", "bsefo/863725", "bsefo/863752", "bsefo/863759", "bsefo/863778", "bsefo/863792", "bsefo/863834", "bsefo/863847", "bsefo/863857", "bsefo/863915", "bsefo/863975", "bsefo/864010", "bsefo/864037", "bsefo/864057", "bsefo/864065", "bsefo/864107", "bsefo/864120", "bsefo/864122", "bsefo/864152", "bsefo/1100438", "bsefo/1100448", "bsefo/863633", "bsefo/863636", "bsefo/863646", "bsefo/863671", "bsefo/863711", "bsefo/863719", "bsefo/863739", "bsefo/863766", "bsefo/863768", "bsefo/863775", "bsefo/863812", "bsefo/863815", "bsefo/863845", "bsefo/863869", "bsefo/863874", "bsefo/863881", "bsefo/863896", "bsefo/863903", "bsefo/863908", "bsefo/863931", "bsefo/863935", "bsefo/863943", "bsefo/863961", "bsefo/863972", "bsefo/863989", "bsefo/864019", "bsefo/864054", "bsefo/864084", "bsefo/864090", "bsefo/864091", "bsefo/864093", "bsefo/864111", "bsefo/864116", "bsefo/864141", "bsefo/864147", "bsefo/872482", "bsefo/889823", "bsefo/863695", "bsefo/863710", "bsefo/863814", "bsefo/863844", "bsefo/863934", "bsefo/863981", "bsefo/863992", "bsefo/864004", "bsefo/864049", "bsefo/873225", "bsefo/889606", "bsefo/863648", "bsefo/863687", "bsefo/863742", "bsefo/863940", "bsefo/863957", "bsefo/863962", "bsefo/863985", "bsefo/864119", "bsefo/873513", "bsefo/873902", "bsefo/877689", "bsefo/860778", "bsefo/861718", "bsefo/863655", "bsefo/863670", "bsefo/863937", "bsefo/863988", "bsefo/864094", "bsefo/864117", "bsefo/889478", "bsefo/889678", "bsefo/889740", "bsefo/860795", "bsefo/860952", "bsefo/861107", "bsefo/861146", "bsefo/861348", "bsefo/861672", "bsefo/863639", "bsefo/863649", "bsefo/861006", "bsefo/861662", "bsefo/860994", "bsefo/863165", "bsefo/888558", "bsefo/861947", "bsefo/863175", "bsefo/861088", "bsefo/861153", "bsefo/860948", "bsefo/861438", "bsefo/889530", "bsefo/1100439", "bsefo/1100133", "bsefo/861152", "bsefo/861437", "bsefo/889016", "bsefo/889385", "bsefo/861033", "bsefo/861277", "bsefo/861496", "bsefo/861214", "bsefo/861335", "bsefo/861215", "bsefo/862995", "bsefo/889727", "bsefo/861479", "bsefo/861548", "bsefo/861813", "bsefo/861801", "bsefo/861327", "bsefo/889302", "bsefo/861850", "bsefo/863010", "bsefo/861248", "bsefo/889527", "bsefo/889722", "bsefo/889939", "bsefo/863187", "bsefo/861715", "bsefo/861424", "bsefo/1100677", "bsefo/888638", "bsefo/861186", "bsefo/888845", "bsefo/861382", "bsefo/861714", "bsefo/860831", "bsefo/862174", "bsefo/861755", "bsefo/1100248", "bsefo/889283", "bsefo/861380", "bsefo/861788", "bsefo/863022", "bsefo/861991", "bsefo/862233", "bsefo/862149", "bsefo/862359", "bsefo/861704", "bsefo/863048", "bsefo/861794", "bsefo/863113", "bsefo/860821", "bsefo/863206", "bsefo/860886", "bsefo/863060", "bsefo/861857", "bsefo/861538", "bsefo/861603", "bsefo/861568", "bsefo/862377", "bsefo/860941", "bsefo/861845", "bsefo/862164", "bsefo/861536", "bsefo/860793", "bsefo/861057", "bsefo/860982", "bsefo/861098", "bsefo/861221", "bsefo/861332", "bsefo/860892", "bsefo/860946", "bsefo/861601", "bsefo/860884", "bsefo/861042", "bsefo/861445", "bsefo/861547", "bsefo/861611", "bsefo/861163", "bsefo/861275", "bsefo/1101017", "bsefo/1101259", "bsefo/1101084", "bsefo/1101219", "bsefo/1101048", "bsefo/1101384", "bsefo/1101464", "bsefo/1101359", "bseeq/999903", "bseeq/999908", "bseeq/999901", "bseeq/500008", "bseeq/500199", "bseeq/500227", "bseeq/500290", "bseeq/500335", "bseeq/500440", "bseeq/500570", "bseeq/500870", "bseeq/500940", "bseeq/501455", "bseeq/505163", "bseeq/505710", "bseeq/506109", "bseeq/507878", "bseeq/508941", "bseeq/513337", "bseeq/513375", "bseeq/514162", "bseeq/514360", "bseeq/517334", "bseeq/523660", "bseeq/524654", "bseeq/531082", "bseeq/531637", "bseeq/532323", "bseeq/532514", "bseeq/532667", "bseeq/532749", "bseeq/532797", "bseeq/532822", "bseeq/532939", "bseeq/532976", "bseeq/533282", "bseeq/533285", "bseeq/533293", "bseeq/533339", "bseeq/534091", "bseeq/534618", "bseeq/539277", "bseeq/539437", "bseeq/539876", "bseeq/540709", "bseeq/540743", "bseeq/540879", "bseeq/543257", "bseeq/543290", "bseeq/543320", "bseeq/543530", "bseeq/543914", "bseeq/543969", "bseeq/544181", "bseeq/544200", "bseeq/544244", "bseeq/544311", "bseeq/544322", "bseeq/544213", "bseeq/543960", "bseeq/543233", "bseeq/543243", "bseeq/543396", "bseeq/542628", "bseeq/539561", "bseeq/539594", "bseeq/534597", "bseeq/533098", "bseeq/533122", "bseeq/532977", "bseeq/532926", "bseeq/532834", "bseeq/532532", "bseeq/532605", "bseeq/532321", "bseeq/532159", "bseeq/530249", "bseeq/530305", "bseeq/530377", "bseeq/524709", "bseeq/524226", "bseeq/517354", "bseeq/519602", "bseeq/500032", "bseeq/999902", "bseeq/999915", "bseeq/999916", "bseeq/999917", "bseeq/999918", "bseeq/999920", "bseeq/999921", "bseeq/999922", "bseeq/999923", "bseeq/999924", "bseeq/999925", "bseeq/999926", "bseeq/999927", "bseeq/999928", "bseeq/999929", "bseeq/999909", "bseeq/999910", "bseeq/999911", "bseeq/999912", "bseeq/999913", "bseeq/999904", "bseeq/999905", "bseeq/999906", "bseeq/999907", "bseeq/999920090", "bseeq/999920092", "bseeq/500110", "bseeq/500820", "bseeq/504614", "bseeq/500520", "bseeq/500117", "bseeq/514142", "bseeq/512247", "bseeq/521194", "bseeq/523411", "bseeq/524715", "bseeq/524735", "bseeq/526861", "bseeq/530007", "bseeq/532175", "bseeq/532406", "bseeq/532407", "bseeq/532757", "bseeq/532966", "bseeq/533272", "bseeq/534748", "bseeq/540135", "bseeq/540180", "bseeq/540762", "bseeq/540395", "bseeq/543237", "bseeq/543984", "bseeq/544045", "bseeq/543523", "bseeq/544249", "bseeq/544274", "bseeq/543994", "bseeq/544021", "bseeq/543227", "bseeq/543220", "bseeq/543066", "bseeq/540530", "bseeq/540699", "bseeq/540953", "bseeq/542230", "bseeq/542066", "bseeq/540079", "bseeq/539574", "bseeq/539519", "bseeq/539195", "bseeq/539083", "bseeq/533269", "bseeq/533096", "bseeq/532898", "bseeq/533301", "bseeq/533385", "bseeq/532775", "bseeq/532610", "bseeq/532628", "bseeq/532390", "bseeq/532218", "bseeq/532123", "bseeq/526967", "bseeq/530475", "bseeq/530233", "bseeq/531241", "bseeq/523676", "bseeq/524204", "bseeq/517344", "bseeq/511760", "bseeq/512599", "bseeq/507944", "bseeq/511196", "bseeq/505700", "bseeq/504903", "bseeq/500547", "bseeq/500288", "bseeq/504351", "bseeq/500086", "bseeq/503310", "bseeq/503663", "bseeq/500171", "bseeq/500251", "bseeq/500253", "bseeq/504973", "bseeq/505750", "bseeq/509557", "bseeq/513361", "bseeq/512267", "bseeq/523598", "bseeq/524711", "bseeq/531784", "bseeq/532163", "bseeq/532187", "bseeq/532329", "bseeq/532686", "nseeq/199", "nseeq/236", "nseeq/422", "nseeq/1008", "nseeq/1333", "nseeq/1922", "nseeq/2029", "nseeq/3103", "nseeq/3507", "nseeq/6964", "nseeq/11195", "nseeq/11287", "nseeq/13976", "nseeq/18143", "nseeq/20329", "nseeq/20906", "nseeq/21174", "nseeq/21551", "nseeq/21770", "nseeq/28899", "nseeq/30835", "nseeq/31163", "nseeq/29666", "nseeq/999920000", "nseeq/999920001", "nseeq/999920039", "nseeq/999920040", "nseeq/999920041", "nseeq/999920042", "nseeq/999920043", "nseeq/999920044", "nseeq/999920045", "nseeq/999920046", "nseeq/999920047", "nseeq/999920048", "nseeq/999920050", "nseeq/999920051", "nseeq/999920052", "nseeq/999920053", "nseeq/999920054", "nseeq/999920055", "nseeq/999920056", "nseeq/999920057", "nseeq/999920058", "nseeq/999920063", "nseeq/999920064", "nseeq/999920065", "nseeq/999920067", "nseeq/999920068", "nseeq/999920069", "nseeq/999920070", "nseeq/999920071", "nseeq/999920072", "nseeq/999920073", "nseeq/999920074", "nseeq/999920076", "nseeq/999920077", "nseeq/999920080", "nseeq/999920084", "nseeq/999920085", "nseeq/999920086", "nseeq/999920003", "nseeq/999920004", "nseeq/999920005", "nseeq/999920006", "nseeq/999920008", "nseeq/999920009", "nseeq/999920010", "nseeq/999920011", "nseeq/999920012", "nseeq/999920013", "nseeq/999920014", "nseeq/999920015", "nseeq/999920016", "nseeq/999920017", "nseeq/999920018", "nseeq/999920020", "nseeq/999920021", "nseeq/999920027", "nseeq/999920028", "nseeq/999920029", "nseeq/999920030", "nseeq/999920031", "nseeq/999920032", "nseeq/999920033", "nseeq/999920024", "nseeq/999920025", "nseeq/29087", "nseeq/29098", "nseeq/29284", "nseeq/29482", "nseeq/27107", "nseeq/27095", "nseeq/27176", "nseeq/27309", "nseeq/27839", "nseeq/21614", "nseeq/21131", "nseeq/20825", "nseeq/23729", "nseeq/24265", "nseeq/24415", "nseeq/24532", "nseeq/24742", "nseeq/24777", "nseeq/24854", "nseeq/24856", "nseeq/25049", "nseeq/25073", "nseeq/25252", "nseeq/25358", "nseeq/25584", "nseeq/25756", "nseeq/25855", "nseeq/20321", "nseeq/20425", "nseeq/20560", "nseeq/19585", "nseeq/19813", "nseeq/20092", "nseeq/18417", "nseeq/18564", "nseeq/18593", "nseeq/19410", "nseeq/18151", "nseeq/18096", "nseeq/17971", "nseeq/17875", "nseeq/17424", "nseeq/17520", "nseeq/16682", "nseeq/13260", "nseeq/13342", "nseeq/13439", "nseeq/13538", "nseeq/13611", "nseeq/14450", "nseeq/14771", "nseeq/14922", "nseeq/14977", "nseeq/15254", "nseeq/15283", "nseeq/15313", "nseeq/15355", "nseeq/15555", "nseeq/11236", "nseeq/11162", "nseeq/11445", "nseeq/11483", "nseeq/11618", "nseeq/10604", "nseeq/10666", "nseeq/11821", "nseeq/11905", "nseeq/11915", "nseeq/11919", "nseeq/13072", "nseeq/6908", "nseeq/6656", "nseeq/6994", "nseeq/7463", "nseeq/7553", "nseeq/7852", "nseeq/7919", "nseeq/7929", "nseeq/5407", "nseeq/5449", "nseeq/5475", "nseeq/5637", "nseeq/5633", "nseeq/5701", "nseeq/5748", "nseeq/6545", "nseeq/8806", "nseeq/8840", "nseeq/9592", "nseeq/9599", "nseeq/9683", "nseeq/9685", "nseeq/10440", "nseeq/3518", "nseeq/3348", "nseeq/3351", "nseeq/3405", "nseeq/3456", "nseeq/3466", "nseeq/3496", "nseeq/3736", "nseeq/3063", "nseeq/3150", "nseeq/3273", "nseeq/2794", "nseeq/2885", "nseeq/2963", "nseeq/4067", "nseeq/4481", "nseeq/4656", "nseeq/4717", "nseeq/4668", "nseeq/4745", "mcxcomm/448589", "mcxcomm/294", "mcxcomm/401", "mcxcomm/556", "mcxcomm/447969", "mcxcomm/446615", "mcxcomm/447622", "mcxcomm/447617", "mcxcomm/447722", "mcxcomm/596", "mcxcomm/437994", "mcxcomm/449147", "mcxcomm/447935", "mcxcomm/447569", "mcxcomm/447621", "mcxcomm/447630", "mcxcomm/447619", "mcxcomm/436581", "mcxcomm/441834", "mcxcomm/451095", "mcxcomm/450780", "mcxcomm/447570", "mcxcomm/441306", "mcxcomm/447724", "mcxcomm/447720", "mcxcomm/447668", "mcxcomm/447671", "mcxcomm/447592", "mcxcomm/447931", "mcxcomm/448993", "mcxcomm/448203", "mcxcomm/447571", "mcxcomm/447623", "mcxcomm/447566", "mcxcomm/447567", "mcxcomm/447564", "mcxcomm/444529", "mcxcomm/441833", "mcxcomm/447629", "mcxcomm/444531", "mcxcomm/440576", "mcxcomm/447620", "mcxcomm/447565", "mcxcomm/448000", "mcxcomm/446496", "mcxcomm/448163", "mcxcomm/450659", "mcxcomm/447937", "mcxcomm/447934", "mcxcomm/447928", "mcxcomm/447932", "mcxcomm/447902", "mcxcomm/447904", "mcxcomm/447561", "mcxcomm/447906", "mcxcomm/433351", "mcxcomm/438425", "mcxcomm/447590", "mcxcomm/447562", "mcxcomm/444536", "mcxcomm/447563", "mcxcomm/447970", "mcxcomm/447674", "mcxcomm/448211", "mcxcomm/447672", "mcxcomm/450845", "mcxcomm/447628", "mcxcomm/433717", "mcxcomm/433701", "mcxcomm/449266", "mcxcomm/447586", "mcxcomm/449003", "mcxcomm/447670", "mcxcomm/446495", "mcxcomm/447669", "mcxcomm/447662", "mcxcomm/449109", "mcxcomm/449027", "mcxcomm/433350", "mcxcomm/439487", "mcxcomm/431876", "mcxcomm/435697", "mcxcomm/449943", "mcxcomm/447936", "mcxcomm/448469", "mcxcomm/447568", "mcxcomm/447933", "mcxcomm/447618", "mcxcomm/447909", "mcxcomm/449453", "mcxcomm/433758", "mcxcomm/433766", "mcxcomm/433762", "mcxcomm/447938", "mcxcomm/434174", "mcxcomm/434765", "mcxcomm/433781", "mcxcomm/434178", "mcxcomm/435312", "mcxcomm/447998", "mcxcomm/447905", "mcxcomm/447613", "mcxcomm/451167", "mcxcomm/451594", "mcxcomm/451348", "mcxcomm/449099", "mcxcomm/449089", "mcxcomm/433802", "mcxcomm/448079", "mcxcomm/446265", "mcxcomm/448618", "mcxcomm/449146", "mcxcomm/448983", "mcxcomm/448467", "mcxcomm/447625", "mcxcomm/447667", "mcxcomm/447994", "mcxcomm/447718", "mcxcomm/447910", "mcxcomm/447976", "mcxcomm/447627", "mcxcomm/449954", "mcxcomm/450784", "mcxcomm/448386", "mcxcomm/447483", "mcxcomm/447663", "mcxcomm/447917", "mcxcomm/447157", "mcxcomm/441305", "mcxcomm/451104", "mcxcomm/447559", "mcxcomm/449079", "mcxcomm/447719", "mcxcomm/448318", "mcxcomm/450657", "mcxcomm/447903", "mcxcomm/444535", "mcxcomm/448162", "mcxcomm/448963", "mcxcomm/449208", "mcxcomm/447925", "mcxcomm/447908", "mcxcomm/447584", "mcxcomm/444205", "mcxcomm/449944", "mcxcomm/447575", "mcxcomm/443450", "mcxcomm/449489", "mcxcomm/443449", "mcxcomm/447554", "mcxcomm/447560", "mcxcomm/434590", "mcxcomm/434025", "mcxcomm/443953", "mcxcomm/444206", "mcxcomm/433813", "mcxcomm/447572", "mcxcomm/447588", "mcxcomm/447631", "mcxcomm/448468", "mcxcomm/449104", "mcxcomm/444542", "mcxcomm/444533", "mcxcomm/447726", "mcxcomm/451305", "mcxcomm/448003", "mcxcomm/447929", "mcxcomm/450872", "mcxcomm/447972", "mcxcomm/447930", "mcxcomm/447901", "mcxcomm/449119", "mcxcomm/433649", "mcxcomm/433777", "mcxcomm/449067", "mcxcomm/450658", "mcxcomm/434252", "mcxcomm/447968", "mcxcomm/449491", "mcxcomm/433705", "mcxcomm/447660", "mcxcomm/447915", "mcxcomm/448084", "mcxcomm/447730", "mcxcomm/447721", "mcxcomm/448002", "mcxcomm/450091", "mcxcomm/450269", "mcxcomm/446488", "mcxcomm/434166", "mcxcomm/433579", "mcxcomm/447633", "mcxcomm/433509", "mcxcomm/447979", "mcxcomm/447926", "mcxcomm/444530", "mcxcomm/446266", "mcxcomm/433571", "mcxcomm/433505", "mcxcomm/447664" });
            String jSubReq = JsonConvert.SerializeObject(subscribeReq);
            string subscribeRes = await CS.SubscribeFeed(jSubReq);
            Console.WriteLine(subscribeRes);


            ////Subscribe to the open interest
            //string jSubOpenInterestReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nseeq/234656" }));
            //string subscribeOpenInterestRes = await CS.SubscribeOpenInterest(jSubOpenInterestReq);
            //Console.WriteLine(subscribeOpenInterestRes);

            ////Subscribe to the LPP
            //string jSubLppReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nsefo/234656" }));
            //string subscribeLppRes = await CS.SubscribeLpp(jSubLppReq);
            //Console.WriteLine(subscribeLppRes);

            ////Subscribe to the market status
            //string jSubMarketStatusReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nseeq", "bsefo" }));
            //string subscribeMarketStatusRes = await CS.SubscribeMarketStatus(jSubMarketStatusReq);
            //Console.WriteLine(subscribeMarketStatusRes);

            ////Subscribe to the high 52 week
            //string jSubHigh52WeekReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "nseeq" }));
            //string subscribeHigh52WeekRes = await CS.SubscribeHigh52week(jSubHigh52WeekReq);
            //Console.WriteLine(subscribeHigh52WeekRes);

            ////Subscribe to the low 52 week
            //string jSubLow52WeekReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "bseeq" }));
            //string subscribeLow52WeekRes = await CS.SubscribeLow52week(jSubLow52WeekReq);
            //Console.WriteLine(subscribeLow52WeekRes);

            ////Subscribe to the upper circuit
            //string jSubUpperCircuitReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "bsefo", "ncdexcomm" }));
            //string subscribeUpperCircuitRes = await CS.SubscribeUpperCircuit(jSubUpperCircuitReq);
            //Console.WriteLine(subscribeUpperCircuitRes);

            ////Subscribe to the lower circuit
            //string jSubLowerCircuitReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "mcxcomm" }));
            //string subscribeLowerCircuitRes = await CS.SubscribeLowerCircuit(jSubLowerCircuitReq);
            //Console.WriteLine(subscribeLowerCircuitRes);

            ////Subscribe to the order updates
            //string jSubOrderUpdatesReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "93080048" }));
            //string subscribeOrderUpdatesRes = await CS.SubscribeOrderUpdates(jSubOrderUpdatesReq);
            //Console.WriteLine(subscribeOrderUpdatesRes);

            ////Subscribe to the trade updates
            //string jSubTradeUpdatesReq = JsonConvert.SerializeObject(new SubscribeReq(new List<string> { "93080048" }));
            //string subscribeTradeUpdatesRes = await CS.SubscribeTradeUpdates(jSubTradeUpdatesReq);
            //Console.WriteLine(subscribeTradeUpdatesRes);

            Thread.Sleep(10000);

            //Unsubscribe to the feed
            UnSubscribeReq unSubscribeReq = new UnSubscribeReq(new List<string> { "bseeq/999904" });
            string jUnSubReq = JsonConvert.SerializeObject(unSubscribeReq);
            string unSubscribeRes = await CS.UnsubscribeFeed(jUnSubReq);
            Console.WriteLine(unSubscribeRes);

            ////Unsubscribe to the open interest
            //string jUnSubOpenInterestReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nseeq/234656" }));
            //string unSubscribeOpenInterestRes = await CS.UnsubscribeOpenInterest(jUnSubOpenInterestReq);
            //Console.WriteLine(unSubscribeOpenInterestRes);

            ////Unsubscribe to the LPP
            //string jUnSubLppReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nsefo/234656" }));
            //string unSubscribeLppRes = await CS.UnsubscribeLpp(jUnSubLppReq);
            //Console.WriteLine(unSubscribeLppRes);

            ////Unsubscribe to the market status
            //string jUnSubMarketStatusReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nseeq", "bsefo" }));
            //string unSubscribeMarketStatusRes = await CS.UnsubscribeMarketStatus(jUnSubMarketStatusReq);
            //Console.WriteLine(unSubscribeMarketStatusRes);

            ////Unsubscribe to the high 52 week
            //string jUnSubHigh52WeekReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "nseeq" }));
            //string unSubscribeHigh52WeekRes = await CS.UnsubscribeHigh52week(jUnSubHigh52WeekReq);
            //Console.WriteLine(unSubscribeHigh52WeekRes);

            ////Unsubscribe to the low 52 week
            //string jUnSubLow52WeekReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "bseeq" }));
            //string unSubscribeLow52WeekRes = await CS.UnsubscribeLow52week(jUnSubLow52WeekReq);
            //Console.WriteLine(unSubscribeLow52WeekRes);

            ////Unsubscribe to the upper circuit
            //string jUnSubUpperCircuitReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "bsefo", "ncdexcomm" }));
            //string unSubscribeUpperCircuitRes = await CS.UnsubscribeUpperCircuit(jUnSubUpperCircuitReq);
            //Console.WriteLine(unSubscribeUpperCircuitRes);

            ////Unsubscribe to the lower circuit
            //string jUnSubLowerCircuitReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "mcxcomm" }));
            //string unSubscribeLowerCircuitRes = await CS.UnsubscribeLowerCircuit(jUnSubLowerCircuitReq);
            //Console.WriteLine(unSubscribeLowerCircuitRes);

            ////Unsubscribe to the order updates
            //string jUnSubOrderUpdatesReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "93080048" }));
            //string unSubscribeOrderUpdatesRes = await CS.UnSubscribeOrderUpdates(jUnSubOrderUpdatesReq);
            //Console.WriteLine(unSubscribeOrderUpdatesRes);

            ////Unsubscribe to the trade updates
            //string jUnSubTradeUpdatesReq = JsonConvert.SerializeObject(new UnSubscribeReq(new List<string> { "93080048" }));
            //string unSubscribeTradeUpdatesRes = await CS.UnSubscribeTradeUpdates(jUnSubTradeUpdatesReq);
            //Console.WriteLine(unSubscribeTradeUpdatesRes);


            Thread.Sleep(100000);

            //Disconnect from the host
            string disconnectRes = await CS.DisconnectHost();
            Console.WriteLine(disconnectRes);

        }

        private static T BuildStruct<T>(byte[] Bytes)
        {
            T myStruct;
            //Console.WriteLine("Bytes: " + Marshal.SizeOf<T>());
            // Convert byte array to struct
            GCHandle handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
            try
            {
                myStruct = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                return myStruct;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
            finally
            {
                handle.Free();
            }
        }

        private static async void printCount()
        {
            while (true)
            {
                Console.WriteLine(DateTime.Now.ToString() + " Count: " + Program.count.ToString());
                Thread.Sleep(15000);
            }
        }

    }

    [Serializable]
    internal class ConnectReq
    {
        public string host { get; set; }
        public int port { get; set; }
        public string token { get; set; }

    }

    internal class SubscribeReq
    {
        private List<string> _subscriptionList = null;

        public SubscribeReq(List<string> topicList)
        {
            _subscriptionList = topicList;
        }

        public List<string> subscriptionList
        {
            get { return _subscriptionList; }
        }

    }

    internal class UnSubscribeReq
    {
        private List<string> _topicList = null;

        public UnSubscribeReq(List<string> topicList)
        {
            _topicList = topicList;
        }

        public List<string> unsubscriptionList
        {
            get { return _topicList; }
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct MWBOCombined
    {
        public int Ltp;
        public uint LastTradedQuantity;
        public uint TradedVolume;
        public int High ;
        public int Low ;
        public int Open ;
        public int Close ;
        public int AverageTradedPrice ;
        public ushort Reserved ;
        public uint BestBidQuantity ;
        public int BestBidPrice ;
        public uint BestAskQuantity ;
        public int BestAskPrice ;
        public uint TotalBidQuantity ;
        public uint TotalAskQuantity ;
        public int PriceDivisor ;
        public int LastTradedTime ;

        [MarshalAs(UnmanagedType.Struct, SizeConst = 10)]
        public Depth MarketDepth;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Depth
    {
        public uint Quantity ;
        public int Price ;
        public short Orders ;
        public short TransactionType ;
    }

    public struct OpenInterestData
    {
        public int OpenInterest ;
        public int DayHighOi ;
        public int DayLowOi ;
        public int PreviousOi ;
    }

    public struct LppData
    {
        public uint LppHigh ;
        public uint LppLow ;
        public int PriceDivisor ;
    }

    public struct UpperCircuitData
    {
        public uint InstrumentId ;
        public uint UpperCircuit ;
        public int PriceDivisor ;
    }

    public struct LowerCircuitData
    {
        public uint InstrumentId ;
        public uint LowerCircuit ;
        public int PriceDivisor ;
    }

    public struct MarketStatusData
    {
        public ushort MarketStatusCode ;
    }

    public struct High52WeekData
    {
        public uint InstrumentId ;
        public uint High52Week ;
        public int PriceDivisor ;
    }

    public struct Low52WeekData
    {
        public uint InstrumentId ;
        public uint Low52Week ;
        public int PriceDivisor ;
    }


}
