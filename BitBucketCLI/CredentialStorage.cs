﻿using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace BitBucketCLI {
    public class CredentialStorage {
        private static readonly string CREDENTIALS_SETTING_KEY = "credentials";
        private static readonly string ENTROPY_SETTING_KEY = "entropy";

        public void StoreCredentials(string server, string user, string password) {
            var entropy = new byte[20];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
                rng.GetBytes(entropy);
            }

            var plaintextData = string.Format("{0}|{1}|{2}", user, password, server);
            var plaintext = Encoding.UTF8.GetBytes(plaintextData);
            var ciphertext = ProtectedData.Protect(plaintext, entropy, DataProtectionScope.CurrentUser);

            var entropy64 = Convert.ToBase64String(entropy);
            var ciphertext64 = Convert.ToBase64String(ciphertext);

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[ENTROPY_SETTING_KEY] == null) {
                config.AppSettings.Settings.Add(new KeyValueConfigurationElement(ENTROPY_SETTING_KEY, entropy64));
            } else {
                config.AppSettings.Settings[ENTROPY_SETTING_KEY].Value = entropy64;
            }
            if (config.AppSettings.Settings[CREDENTIALS_SETTING_KEY] == null) {
                config.AppSettings.Settings.Add(new KeyValueConfigurationElement(CREDENTIALS_SETTING_KEY, ciphertext64));
            } else {
                config.AppSettings.Settings[CREDENTIALS_SETTING_KEY].Value = ciphertext64;
            }
            config.Save(ConfigurationSaveMode.Modified);
        }

        public bool TryGetCredentials(out string server, out string user, out string password) {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var entropy64 = config.AppSettings.Settings[ENTROPY_SETTING_KEY]?.Value;
            var ciphertext64 = config.AppSettings.Settings[CREDENTIALS_SETTING_KEY]?.Value;
            
            if (entropy64 == null || ciphertext64 == null) {
                server = null;
                user = null;
                password = null;
                return false;
            }

            var entropy = Convert.FromBase64String(entropy64);
            var ciphertext = Convert.FromBase64String(ciphertext64);

            var plaintext = ProtectedData.Unprotect(ciphertext, entropy, DataProtectionScope.CurrentUser);
            var plaintextData = Encoding.UTF8.GetString(plaintext);
            var data = plaintextData.Split('|');
            server = data[0];
            user = data[1];
            password = data[2];

            return true;
        }
    }
}
