import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  root: 'renderer',
  build: {
    outDir: '../build/renderer',
    emptyOutDir: true
  },
  plugins: [react()]
});

