using Amazon.S3.Model;
using Amazon.S3;

namespace Pastebin_api.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "pastebin-like";

        public S3Service(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task UploadTextAsync(string key, string content)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                ContentBody = content
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
        }
        public async Task<string> GetTextAsync(string key)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
            };
            using (var response = await _s3Client.GetObjectAsync(getRequest))
            using (var responseStream = response.ResponseStream)
            using (var reader = new StreamReader(responseStream))
            {
                string content = await reader.ReadToEndAsync();
                return content;
            }
        }
        public async Task DeleteTextAsync(string key)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }
    }
}
