namespace csi5112group1project_service.Services;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

public class AwsService
{
  private const string bucketName = "csi5112project-image-bucket";
  private static readonly RegionEndpoint bucketRegion = RegionEndpoint.CACentral1;
  private static IAmazonS3 _client;

  public AwsService()
  {
    _client = new AmazonS3Client(bucketRegion);
  }

  public async Task<bool> UploadImageAsync(string base64ImageString, string key)
  {
    var result = false;
    try
    {
      var bytes = Convert.FromBase64String(base64ImageString);
      var image = new MemoryStream(bytes);
      var putRequest = new PutObjectRequest
      {
        BucketName = bucketName,
        Key = key,
        InputStream = image,
        ContentType = "image/png"
      };

      // putRequest.Metadata.Add("x-amz-meta-title", "someTitle");
      PutObjectResponse response = await _client.PutObjectAsync(putRequest);
      if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
      {
        result = true;
      }

    }
    catch (AmazonS3Exception e)
    {
      Console.WriteLine(
              "Error encountered ***. Message:'{0}' when writing an object"
              , e.Message);
    }
    catch (Exception e)
    {
      Console.WriteLine(
          "Unknown encountered on server. Message:'{0}' when writing an object"
          , e.Message);
    }
    return result;
  }
}
