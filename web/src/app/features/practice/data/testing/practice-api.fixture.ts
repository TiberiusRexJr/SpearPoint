import { PracticeQuestionDto, StartResponse } from '../practice.models';

export function makeQuestion(overrides: Partial<PracticeQuestionDto> = {}): PracticeQuestionDto {
  return {
    id: 'q-fixture',
    section: 'ArithmeticReasoning',
    stem: 'Fixture question?',
    choices: ['A','B','C','D'],
    answerIndex: 0,
    ...overrides
  };
}

export function makeStartResponse(total = 3): StartResponse {
  return { sessionId: 'sess-fixture', firstQuestion: makeQuestion(), total };
}
