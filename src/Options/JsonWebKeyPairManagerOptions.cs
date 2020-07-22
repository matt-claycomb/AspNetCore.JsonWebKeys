namespace AspNetCore.JsonWebKeys.Options
{
    public class JsonWebKeyPairManagerOptions
    {
        public int KeyLifetimeDays { get; set; } = 90;
        public bool DisableKeyRotation { get; set; } = false;
    }
}
