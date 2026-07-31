// Storybook 스토리 예제. 실제 Storybook 프로젝트에서 사용하는 파일입니다.
import { Button, Badge } from './App'

export default {
  title: 'DesignSystem/Button',
  component: Button,
}

export const Primary = () => <Button>저장</Button>
export const Outline = () => <Button variant="outline">취소</Button>
export const Danger = () => <Button variant="danger">삭제</Button>
export const Disabled = () => <Button disabled>비활성</Button>

export const BadgeTones = () => (
  <>
    <Badge>기본</Badge>
    <Badge tone="success">완료</Badge>
    <Badge tone="danger">오류</Badge>
  </>
)
