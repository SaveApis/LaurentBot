import './ui/assets/css/main.css'
import './ui/assets/css/tailwindcss.css'

import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import AppView from './ui/views/AppView.tsx'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <AppView/>
    </StrictMode>,
)
