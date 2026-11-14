export function formatDate(strDate) {

    const [year, month, day] = strDate.split("-").map(Number);

    const date = new Date(year, month - 1, day);

    const formatter = new Intl.DateTimeFormat("en-GB", {
        day: "2-digit",
        month: "short",
        year: "numeric"
    });

    return formatter.format(date);
}
