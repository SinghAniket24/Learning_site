// Services/SupabaseService.cs
using Supabase;
using System;
using System.Threading.Tasks;

namespace Learning_site.Services
{
    public class SupabaseService
    {
        public Client Client { get; private set; }

        public async Task InitializeAsync()
        {
            var url = "https://utmvmwavtknhateaflfx.supabase.co"; // your Supabase URL
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InV0bXZtd2F2dGtuaGF0ZWFmbGZ4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTYxODA3MzAsImV4cCI6MjA3MTc1NjczMH0.qqWHkhV5Is2fevMDtLoTrgHBuSdGPwh-lQyfMDNj5jQ"; // your Supabase anon key

            Client = new Client(url, key);
            await Client.InitializeAsync();
        }
    }
}
