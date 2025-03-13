namespace AlzaBoxApiTests;

[TestFixture]
public class AlzaBoxTests : TestBase
{
    private AlzaBoxApiClient _apiClient;

    [SetUp]
    public void Init() => _apiClient = new AlzaBoxApiClient(Configuration);

    private async Task<string> GetRandomAvailableBoxIdAsync()
    {
        var boxes = (await _apiClient.GetAsync("boxes"))["data"] as JArray;
        return boxes[new Random().Next(boxes.Count)]["id"].ToString();
    }

    [Test]
    public async Task CreateReservation_WithValidData_ShouldSucceed()
    {
        var reservationData = new {
            id = $"RES{Guid.NewGuid()}",
            boxId = await GetRandomAvailableBoxIdAsync(),
            barcode = "1234567890123",
            opener = "PART48"
        };
        
        var result = await _apiClient.PostAsync("reservations", reservationData);
        
        Logger.LogInformation("Ответ API: {Response}", result.ToString());
        
        Assert.AreEqual(reservationData.id, result["id"].ToString());
    }
}
