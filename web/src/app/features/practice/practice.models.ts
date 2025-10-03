export type Section = 'ArithmeticReasoning' | 'WordKnowledge';

export interface PracticeRequestDto { section?: Section; numQuestions: number; }
export interface PracticeQuestionDto {
  id: string;
  section: Section ;
  stem: string;
  choices: string[];
}

export interface InternalQuestionDto extends PracticeQuestionDto {
  answerIndex: number;
}

export interface StartResponse { sessionId: string; firstQuestion?: PracticeQuestionDto | null; total: number; }
export interface AnswerRequest { questionId: string; selectedIndex: number; }
export interface AnswerResponse { isCorrect: boolean; nextQuestion?: PracticeQuestionDto | null; done: boolean; }
export interface ResultDto { correct: number; total: number; percent: number; }
