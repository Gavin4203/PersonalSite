import { formatDate } from './FormatDate';

export function endDateCheck(startDate, endDate) {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const today = new Date();

    if (isNaN(end) || end > today || end < start) {
        return "Now";
    }

    return formatDate(endDate);
}
