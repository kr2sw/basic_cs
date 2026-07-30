type Color = 'red' | 'blue' | 'green'
type Size = 'small' | 'medium' | 'large'

type ProductOption = `${Color}-${Size}`

type EventName = 'click' | 'focus' | 'blur'
type EventHandler = `on${Capitalize<EventName>}`

type CssProperty = 'margin' | 'padding' | 'border'
type CssDirection = 'top' | 'right' | 'bottom' | 'left'
type CssDeclaration = `${CssProperty}-${CssDirection}`

const option: ProductOption = 'red-large'
const handler: EventHandler = 'onClick'
const css: CssDeclaration = 'margin-top'

console.log(option, handler, css)
