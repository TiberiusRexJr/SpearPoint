import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { PracticeComponent } from './features/asvab/practice.component';
import { ResultsComponent } from './features/results/results.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'practice', component: PracticeComponent },
  { path: 'results', component: ResultsComponent },
  { path: '**', redirectTo: '' }
];
