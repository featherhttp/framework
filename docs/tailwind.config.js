/*
** TailwindCSS Configuration File
**
** Docs: https://tailwindcss.com/docs/configuration
** Default: https://github.com/tailwindcss/tailwindcss/blob/master/stubs/defaultConfig.stub.js
*/
module.exports = {
  theme: {
    extend: {
      maxHeight: {
        none: '0'
      },
      width: {
        68: '20rem'
      }
    }
  },
  variants: {},
  plugins: [require('tailwindcss-transitions')()]
}
