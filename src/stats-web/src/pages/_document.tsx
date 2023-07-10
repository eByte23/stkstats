import { Navbar } from '@/components/Navbar'
import { Html, Head, Main, NextScript } from 'next/document'

export default function Document() {
  return (
    <Html lang="en">
      <Head />
      <body className='bg-white min-h-[100vh] dark:bg-slate-900'>
        <Navbar />
        <main id="content" role="main" className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <Main />
        </main>
        <NextScript />
      </body>
    </Html>
  )
}
