// wwwroot/js/site.js

// --- Supabase Initialization ---
const supabaseUrl = 'https://utmvmwavtknhateaflfx.supabase.co'; // Your real URL
const supabaseKey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InV0bXZtd2F2dGtuaGF0ZWFmbGZ4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTYxODA3MzAsImV4cCI6MjA3MTc1NjczMH0.qqWHkhV5Is2fevMDtLoTrgHBuSdGPwh-lQyfMDNj5jQ'; // Your real anon public key

const { createClient } = supabase;
const supabaseClient = createClient(supabaseUrl, supabaseKey);


// --- Robust Authentication State Handler ---
supabaseClient.auth.onAuthStateChange(async (event, session) => {
    const publicPages = ['/login', '/signup'];
    const currentPath = window.location.pathname;

    if (session) {
        // USER IS LOGGED IN
        // Sync the session with the server
        await fetch('/api/auth/setsession', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                accessToken: session.access_token,
                refreshToken: session.refresh_token,
            }),
        });

        // If on a public page, redirect to the dashboard
        if (publicPages.includes(currentPath)) {
            window.location.href = '/dashboard';
        }
    }
});


/**
 * Initiates the Google OAuth sign-in/sign-up flow.
 */
async function initiateGoogleOAuth() {
    await supabaseClient.auth.signInWithOAuth({
        provider: 'google'
    });
}

/**
 * Signs the user out and redirects to the login page.
 */
async function logout() {
    await supabaseClient.auth.signOut();
    window.location.href = '/login';
}