/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './LuxurySalon.Web/Views/**/*.cshtml',
        './LuxurySalon.Web/wwwroot/**/*.html',
        './LuxurySalon.Web/wwwroot/**/*.js',
    ],
    theme: {
        extend: {
            colors: {
                luxury: {
                    gold: '#D4AF37',
                    black: '#0A0A0A',
                    charcoal: '#333333',
                    cream: '#F5F5DC',
                }
            },
            fontFamily: {
                serif: ['Playfair Display', 'serif'],
                sans: ['Inter', 'sans-serif'],
            }
        },
    },
    plugins: [],
}
