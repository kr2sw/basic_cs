import { Logger, createConsoleLogger, DEFAULT_PREFIX } from './logger'

const logger: Logger = createConsoleLogger(DEFAULT_PREFIX)
logger.log('Application started')
logger.log('Loading modules...')

const devLogger = createConsoleLogger('Dev')
devLogger.log('Debug message')
