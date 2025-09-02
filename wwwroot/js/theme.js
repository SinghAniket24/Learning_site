// theme.js

/* --- CSS as JS variables --- */
const style = document.createElement('style');
style.innerHTML = `
  :root {
    --bg-color: #ffffff;
    --text-color: #000000;
    --primary-color: #007bff;
    --card-bg: #f8f9fa;
  }

  body {
    background: var(--bg-color);
    color: var(--text-color);
    transition: all 0.3s;
  }

  .navbar, .btn {
    background: var(--primary-color) !important;
    color: var(--text-color) !important;
  }

  .card {
    background: var(--card-bg);
    transition: all 0.3s;
  }

  a, a:hover {
    color: var(--primary-color);
  }
`;
document.head.appendChild(style);

/* --- Theme definitions --- */
const themes = {
    light: {
        '--bg-color': '#ffffff',
        '--text-color': '#000000',
        '--primary-color': '#007bff',
        '--card-bg': '#f8f9fa'
    },
    dark: {
        '--bg-color': '#1a1a1a',
        '--text-color': '#ffffff',
        '--primary-color': '#4f46e5',
        '--card-bg': '#2c2c2c'
    },
    gradient: {
        '--bg-color': 'linear-gradient(135deg, #4f46e5, #3b82f6, #06b6d4)',
        '--text-color': '#ffffff',
        '--primary-color': '#2563eb',
        '--card-bg': 'rgba(255, 255, 255, 0.1)'
    }
};

/* --- Apply a theme --- */
function applyTheme(themeName) {
    const theme = themes[themeName];
    for (let prop in theme) {
        document.documentElement.style.setProperty(prop, theme[prop]);
    }
    localStorage.setItem('selectedTheme', themeName);
}

/* --- Load saved theme on page load --- */
document.addEventListener('DOMContentLoaded', () => {
    const savedTheme = localStorage.getItem('selectedTheme') || 'light';
    applyTheme(savedTheme);
});

/* --- Button function to toggle theme --- */
function toggleTheme() {
    const current = localStorage.getItem('selectedTheme') || 'light';
    const next = current === 'light' ? 'dark' : current === 'dark' ? 'gradient' : 'light';
    applyTheme(next);
}
