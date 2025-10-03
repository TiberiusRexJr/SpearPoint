import { Injectable } from '@angular/core';
import { of, delay, Observable } from 'rxjs';
import { v4 as uuid } from 'uuid';
import { PracticeApiPort } from './practice-api.port';
import {
  PracticeRequestDto, StartResponse, AnswerRequest, AnswerResponse, ResultDto, PracticeQuestionDto
} from '../practice.models';

/**
 * Deterministic mock for UI-only development and unit tests.
 * Not used until we explicitly register it in providers.
 */
@Injectable({ providedIn: 'root' })
export class PracticeApiMock extends PracticeApiPort {
  private sessions = new Map<string, { questions: PracticeQuestionDto[]; answers: number[] }>();

  start(dto: PracticeRequestDto): Observable<StartResponse> {
    const sessionId = uuid();

    const questions = this.mockQuestions(dto.section, dto.numQuestions);

    // Initialize a new session with empty answers
    this.sessions.set(sessionId, { questions, answers: [] });

    return of({
      sessionId,
      firstQuestion: questions[0],
      total: questions.length
    }).pipe(delay(120)); // simulate async delay
  }

  answer(sessionId: string, request: AnswerRequest): Observable<AnswerResponse> {
    const session = this.sessions.get(sessionId)!;

    const currentIndex = session.answers.length;

    // Record the user’s selected answer
    session.answers.push(request.selectedIndex);

    // Determine if all questions have been answered
    const isFinished = session.answers.length >= session.questions.length;

    // Get the next question if not finished
    const nextQuestion = isFinished ? null : session.questions[currentIndex + 1];

    // Check correctness of the current answer
    const isCorrect = session.questions[currentIndex].answerIndex === request.selectedIndex;

    return of({
      isCorrect,
      nextQuestion: nextQuestion ?? undefined,
      done: isFinished
    }).pipe(delay(80));
  }

  result(sessionId: string): Observable<ResultDto> {
    const session = this.sessions.get(sessionId)!;

    // Count how many answers were correct
    const correctAnswers = session.questions.reduce((count, question, index) => {
      const userAnswer = session.answers[index];
      const isCorrect = question.answerIndex === userAnswer;
      return count + (isCorrect ? 1 : 0);
    }, 0);

    return of({
      correct: correctAnswers,
      total: session.questions.length,
      percent: Math.round((correctAnswers / session.questions.length) * 100)
    }).pipe(delay(50));
  }

  private mockQuestions(
    _section: PracticeQuestionDto['section'] | undefined,
    numQuestions = 5
  )
