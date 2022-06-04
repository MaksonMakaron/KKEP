SELECT s.LastName, AVG(CAST(m.MarkValue as decimal)), d.DisciplineName
FROM     Student s INNER JOIN
                  Mark m ON s.Id = m.IdStudent INNER JOIN
                  Discipline d ON m.IdDiscipline = d.Id
				  WHERE s.IdGroup = 1
				  GROUP BY m.IdDiscipline, s.LastName, d.DisciplineName
