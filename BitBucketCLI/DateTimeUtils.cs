using System;

namespace BitBucketCLI {
    public class DateTimeUtils {
        public static readonly DateTime EPOCH = new DateTime(1970, 01, 01);

        public static DateTime FromEpochMilliseconds(string value) {
            return FromEpochMilliseconds(long.Parse(value));
        }

        public static DateTime FromEpochMilliseconds(long value) {
            return EPOCH + TimeSpan.FromMilliseconds(value);
        }
    }
}
