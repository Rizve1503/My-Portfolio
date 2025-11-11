import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import './index.css'
import Home from './pages/Home.tsx'
import EmeraldTheme from './theme-preview/emerald.tsx'
import PurpleTheme from './theme-preview/purple.tsx'
import Layout from './components/Layout.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/theme-preview/emerald" element={<EmeraldTheme />} />
          <Route path="/theme-preview/purple" element={<PurpleTheme />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  </StrictMode>,
)
