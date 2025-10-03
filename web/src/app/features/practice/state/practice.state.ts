import { signal } from '@angular/core';

export const currentSessionId = signal<string | null>(null);
export const currentQuestionIndex = signal(0);
// Add more signals later; no runtime dependency yet.
