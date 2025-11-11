import { useEffect, useState } from 'react';
import type { ReactNode } from 'react';
import { Moon, Sun } from 'lucide-react';

interface LayoutProps {
  children: ReactNode;
}

function Layout({ children }: LayoutProps) {
  const [mode, setMode] = useState<'dark' | 'light'>('dark');

  useEffect(() => {
    const savedMode = (localStorage.getItem('theme') as 'dark' | 'light' | null) ?? 'dark';
    setMode(savedMode);
    document.documentElement.classList.toggle('dark', savedMode === 'dark');
    if (!localStorage.getItem('theme')) {
      localStorage.setItem('theme', savedMode);
    }
  }, []);

  const toggleTheme = () => {
    const nextMode = mode === 'dark' ? 'light' : 'dark';
    setMode(nextMode);
    document.documentElement.classList.toggle('dark', nextMode === 'dark');
    localStorage.setItem('theme', nextMode);
  };

  return (
    <div className="min-h-screen transition-colors duration-300">
      <header className="fixed top-0 left-0 right-0 flex justify-end p-6 z-50">
        <button
          type="button"
          onClick={toggleTheme}
          className="cursor-pointer transition-opacity duration-200 hover:opacity-80 text-[var(--text-secondary)]"
          aria-label="Toggle theme"
        >
          {mode === 'dark' ? <Sun size={22} /> : <Moon size={22} />}
        </button>
      </header>
      {children}
    </div>
  );
}

export default Layout;
