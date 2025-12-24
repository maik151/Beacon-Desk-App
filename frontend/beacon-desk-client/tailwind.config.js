/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  darkMode: 'class', // Importante para controlar el tema manualmente
  theme: {
    extend: {
      // Aquí conectamos Tailwind con tus fuentes locales
      fontFamily: {
        sans: ['Inter', 'system-ui', 'sans-serif'],
      },
      // Aquí conectamos Tailwind con tus Variables CSS
      colors: {
        primary: {
          DEFAULT: 'var(--primary-color)',
          hover: 'var(--primary-hover)',
        },
        surface: {
          DEFAULT: 'var(--bg-page)', // bg-surface
          input: 'var(--bg-input)',  // bg-surface-input
        },
        border: {
          DEFAULT: 'var(--border-input)',
        },
        text: {
          main: 'var(--text-main)',
          secondary: 'var(--text-secondary)',
          inverse: 'var(--text-inverse)',
        },
        error: 'var(--error-color)',
      }
    },
  },
  plugins: [],
}