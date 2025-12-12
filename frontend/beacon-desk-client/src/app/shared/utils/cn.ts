import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

/**
 * Combina clases de CSS condicionales y resuelve conflictos de Tailwind.
 * @param inputs Lista de clases o condiciones
 * @returns String de clases optimizadas
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}