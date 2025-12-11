/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}", // Esto le dice a Tailwind que busque clases en tus HTML y TS
  ],
  theme: {
    extend: {
      colors: {
        // Aquí definimos tus colores personalizados (basados en tu idea de Figma)
        primary: {
          DEFAULT: '#2563EB', 
          hover: '#1D4ED8',
          content: '#FFFFFF'
        },
        secondary: {
          DEFAULT: '#64748B', 
          hover: '#475569',
          content: '#FFFFFF'
        },
        danger: {
          DEFAULT: '#EF4444',
          hover: '#DC2626',
        }
      },
    },
  },
  plugins: [],
}