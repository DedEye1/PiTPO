const apiBase = '/api/simulation';
let timer = null;

async function fetchState() {
  const response = await fetch(`${apiBase}/state`);
  return await response.json();
}

function drawGrid(state) {
  const canvas = document.getElementById('gridCanvas');
  const ctx = canvas.getContext('2d');
  const size = state.gridSize;
  const cellW = canvas.width / size;
  const cellH = canvas.height / size;

  // очистка
  ctx.fillStyle = 'white';
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  for (const cell of state.grid) {
    const { x, y, entityType } = cell;
    let color = '#fff';
    if (entityType === 'Plant') color = '#2ecc71';
    else if (entityType === 'Herbivore') color = '#f1c40f';
    else if (entityType === 'Carnivore') color = '#e74c3c';
    else continue;

    ctx.fillStyle = color;
    ctx.fillRect(y * cellW, x * cellH, cellW, cellH);
    ctx.strokeStyle = '#ccc';
    ctx.strokeRect(y * cellW, x * cellH, cellW, cellH);
  }
}

function updateStats(state) {
  const stats = state.statistics;
  document.getElementById('stepValue').innerText = state.step;
  document.getElementById('plants').innerText = stats.plants;
  document.getElementById('herbivores').innerText = stats.herbivores;
  document.getElementById('carnivores').innerText = stats.carnivores;
  document.getElementById('eatenPlants').innerText = stats.totalEatenPlants;
  document.getElementById('eatenHerb').innerText = stats.totalEatenHerbivores;
}

async function refresh() {
  const state = await fetchState();
  drawGrid(state);
  updateStats(state);
  // можно обновить графики (Chart.js)
}

// Управление
document.getElementById('stepBtn').onclick = async () => {
  await fetch(`${apiBase}/step`, { method: 'POST' });
  refresh();
};
document.getElementById('startBtn').onclick = async () => {
  const interval = parseInt(document.getElementById('interval').value, 10);
  await fetch(`${apiBase}/start?intervalMs=${interval}`, { method: 'POST' });
  if (timer) clearInterval(timer);
  timer = setInterval(refresh, interval);
};
document.getElementById('stopBtn').onclick = async () => {
  await fetch(`${apiBase}/stop`, { method: 'POST' });
  if (timer) clearInterval(timer);
};
document.getElementById('resetBtn').onclick = async () => {
  await fetch(`${apiBase}/reset`, { method: 'POST' });
  refresh();
};

refresh();