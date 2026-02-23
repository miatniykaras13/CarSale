"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.parseBoolean = parseBoolean;
function parseBoolean(value) {
    if (typeof value === 'boolean') {
        return value;
    }
    if (typeof value === 'string') {
        const lowerValue = value.trim().toLowerCase();
        if (lowerValue === 'true') {
            return true;
        }
        if (lowerValue === 'false') {
            return false;
        }
    }
    throw new Error(`Не удалось преобразовать значение "${value}" в логическое значение.`);
}
//# sourceMappingURL=parse-boolean.util.js.map